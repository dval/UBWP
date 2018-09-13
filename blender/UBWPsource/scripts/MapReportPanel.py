'''
Special thanks to VincentG @ bbug for helping with this.
https://bbug.be/?topic=script-how-to-add-custom-list-property/#post-1280
(4 years ago... the answer was posted to the question I had yesterday. )
'''
import bpy
import mathutils
from bpy.props import FloatVectorProperty, StringProperty

# Create a Prooperty Group. This is quite similar to the idea of a Struct.
# The Property Group is given 2 properties, each cast to a type by the  
# associated property editor. 
class gs_map_point_properties(bpy.types.PropertyGroup):
	name = bpy.props.StringProperty(name="", default="Layer")
	point = bpy.props.FloatVectorProperty(name="")


# all scenes can access the properties.
def initSceneProperties():
	# File path matches the Unity Example
	bpy.types.Scene.gs_FilePath = StringProperty(
		name = "Report Path", 
		default = "../../MapReportExample.txt"
		)
	# Now, create and instance of the Property Group, so there is something 
	# to write to and edit.
	bpy.types.Scene.gs_map_points = bpy.props.CollectionProperty(type=gs_map_point_properties)

	return

# our currently selected mappoint, for visual ref in UIList
class active_map_point(bpy.types.PropertyGroup):
	bpy.types.Object.index = bpy.props.IntProperty(
		default = 0,
		min = 0
		)

# This draws the actuall list in the panel. It's responsible displaying the 
# Property Group as a collection of like items.
class map_point_UIList(bpy.types.UIList):

	## build a button with unique name and value for each Interest Point
	def Button(self,id,value,col,index):
		btn = col.operator("map_jump.button", text=str(id))
		btn.b_vect = value
		btn.b_index = index
		#bpy.data.objects['Cube'].index = index
		#return btn

	# The draw_item function is called for each item of the collection that is visible in the list.
	def draw_item(self, context, layout, data, item, icon, active_data, active_propname, index):
		# this is the actual row that gets drawn
		self.Button("Go", item.point, layout, index)
		layout.prop(item, "name")
		layout.prop(item, "point")

#The 'search' for the 3D viewport is purposfully obtuse. Written 
#this way, it finds the active 3D viewport from any other viewport  
#or panel. This allows the panel to be placed to suit workflow.
class map_point_jump_button(bpy.types.Operator):
	bl_idname = "map_jump.button"
	bl_label = "0"
	b_vect = bpy.props.FloatVectorProperty()
	b_index = bpy.props.IntProperty()
	
	#this to find a window from another window in blender
	def execute(self, context):
		#find our cursor
		context.scene.cursor_location = self.b_vect
		#find current item in list
		context.object.index = self.b_index
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
 

# Now we can use this list anywhere in Blender. In this case, adding it to
# the 3D widnow, as an practical list. 
class MapReportPanel(bpy.types.Panel):
	bl_label = "Map Report"
	bl_idname = "VIEW_3D_PT_map_report_reader"
	bl_space_type = 'VIEW_3D'
	bl_region_type = 'UI'
	bl_context = "view3d"

	def draw(self, context):
		layout = self.layout
		scn = context.scene

		#UI for file name
		layout.label(text="Report File:", icon="FILESEL")
		row = layout.row(); row.prop(scn,"gs_FilePath")

		# display number of items
		layout.prop(scn, "gs_map_points")

		# display list of item
		layout = layout.column()
		layout.template_list("map_point_UIList", "", scn, "gs_map_points", context.object, "index")


def register():
	# Then, register the Property Group immediately for use.
	bpy.utils.register_class(map_point_UIList)
	bpy.utils.register_module(__name__)

def unregister():
	# Then, register the Property Group immediately for use.
	bpy.utils.unregister_class(map_point_UIList)
	bpy.utils.unregister_module(__name__)



if __name__ == "__main__":
	register()
	initSceneProperties()

	scn = bpy.context.scene

	#look for filename relative to blender working folder
	gs_fp = bpy.path.abspath("//"+scn.gs_FilePath)

	#read into list
	with open(gs_fp) as f:
		content = f.readlines()
	#clean lines
	content = [x.strip() for x in content]
	#itterate each line as a new Point of Interest

	# clear scene data
	scn.gs_map_points.clear()

	# rebuild scene data
	for c in range(0,len(content)):
		poi = content[c].split(",",1)
		vecname = poi[0]
		#string to tuple, so python reads as single object cast to vector
		vec = (tuple(float(i) for i in poi[1].strip(" ()").split(",")))
		#build button with data
		my_item = bpy.context.scene.gs_map_points.add()
		my_item.name =  vecname
		my_item.point = vec
