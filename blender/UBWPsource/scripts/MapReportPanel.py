import bpy, socket
from bpy.props import FloatVectorProperty,StringProperty

#region ModuleProps

# Create RNA properties in Scene, so 
# all scenes can access the properties.
def initSceneProperties():
	# File path matches the Unity Example
	path = bpy.types.Scene.gs_FilePath = StringProperty(
		name = "Report Path", 
		default = "../../MapReportExample.txt"
		)
	
	return

#initialize the stuff in RNA
initSceneProperties()

#end region

#region FileMethods

#end region

#region PanelUI

class GameSolidsMapReportPanel(bpy.types.Panel):
	"""Creates a Panel in the 3DView Properties Panel (N-Key)"""
	bl_label = "Map Report"
	bl_idname = "VIEW_3D_PT_map_report_reader"
	bl_space_type = 'VIEW_3D'
	bl_region_type = 'UI'
	bl_context = "view3d"

	## build a button with unique name and value for each Interest Point
	def Button(self,id,value,col):
		btn = col.operator("vector_jump.button", text="Area "+str(id))
		btn.b_vect = value
		return btn

	## build the UI for the Panel
	def draw(self, context):
		layout = self.layout
		scn = context.scene

		#UI for file name
		layout.label(text="Report File:", icon="FILESEL")
		row = layout.row(); row.prop(scn,"gs_FilePath")

		#UI for list of Interest Points
		layout.label(text="Area List:", icon="COLLAPSEMENU")
		col = layout.column(); col.alignment = 'LEFT'

		#look for filename relative to blender working folder
		gs_fp = bpy.path.abspath("//"+scn.gs_FilePath)
		#read into list
		with open(gs_fp) as f:
			content = f.readlines()
		#clean lines
		content = [x.strip() for x in content]
		#itterate each line as a new Point of Interest
		for c in range(0,len(content)):
			poi = content[c].split(",",1)
			vecname = poi[0]
			#string to tuple, so python reads as single object cast to vector
			vec = (tuple(float(i) for i in poi[1].strip("()").split(",")))
			#build button with data
			self.Button(poi[0], vec, col)
			

#The 'search' for the 3D viewport is purposfully obtuse. Written 
#this way, it finds the active 3D viewport from any other viewport  
#or panel. This allows the panel to be placed to suit workflow.
class Vector_Jump_Button(bpy.types.Operator):
	bl_idname = "vector_jump.button"
	bl_label = "0"
	b_vect = bpy.props.FloatVectorProperty()
	
	#this to find a window from another window in blender
	def execute(self, context):
		#find our cursor
		context.scene.cursor_location = self.b_vect
		#find our active 3dView port
		for area in bpy.context.screen.areas:
			if area.type == 'VIEW_3D':
				for region in area.regions:
					if region.type == 'WINDOW':
						#move cursor to Interest point, update view port
						context_override = bpy.context.copy()
						context_override['area'] = area
						context_override['region'] = region
						bpy.ops.view3d.view_center_cursor(context_override)
		
		return{'FINISHED'}    
 

#end region



def register():
	bpy.utils.register_class(Vector_Jump_Button)
	bpy.utils.register_class(GameSolidsMapReportPanel)


def unregister():
	bpy.utils.unregister_class(Vector_Jump_Button)
	bpy.utils.unregister_class(GameSolidsMapReportPanel)


if __name__ == "__main__":
	register()
