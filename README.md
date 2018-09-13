# UBWP
Unity &lt;-> Blender World Position mapper flagger tagger

UBWP was built as a map tool for flagging problem areas in large landscapes. It allows users to record a Object's position in Unity, and jump to that recorded position in Blender by recording positions to an external file. Blender reads from the list of points stored in the file, and can easily navigate to problem area in the source.  ( Eventually Blender will also be able to edit the external file. Then you can pass notes in both directions. . .)

## Basic Use
In Unity, you use a `Recorder Object` which can be the player character or an Empty transform that can move around easily. As you find edits that need to be made, move the Recorder Object to that spont and hit the shortcut for 'record point'. The default is `P-Key`.

After switching back to the source blend, each of the Points recorded will show up as a Button in the the Properties Panel of 3DView. You can then easily navigate to that specific point on the landscape.

### Unity Panel

1. Add the RecordWorldPosition Component to an object. Link to a transform in the scene. This will expose the method `RecordPosition()` that you can call as you like.
2. Optionally, add the KeyListener Component to use a KeyPress to trigger the RecordPostion method.
	
	![Unity Component Stack](./UnityUI.png?raw=true)

### Blender Panel
1. Open the script `MapReportTool.py` in your blender file and click `Run Script`.
2. Open 3D View Properties Panel `N-Key` and enter the file to read from.

	![Blender MapTool Panel](./BlenderUI.png?raw=true)


