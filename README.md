# UBWP
Unity &lt;-> Blender World Position mapper flagger tagger

UBWP was originally built as a map tool for flagging problem areas in large landscapes. It allows users to record a Object's position in Unity, and jump to that recorded position in Blender. It probably has other uses to. ( and will eventually move in both directions. . .)


## Basic Use
In Unity, keep the Recorder Object as the player transform, or an Empty transform that can move around easily. As you find edits that need to be made, move the Recorder Object to that spont and hit the `P-Key`.

When working in the source blend, each of these Interest Points recorded, will show up as a Button in the the Properties Panel of 3DView. You can then click to that specific point on the landscape.

### Unity Install

1. Add the RecordWorldPosition Component to an object. Link to a transform in the scene. This will expose the method `RecordPosition()` that you can call as you like.
1. Optionally, add the KeyListener Component to use a KeyPress to trigger the RecordPostion method.
	![Unity Component Stack](https://www.dropbox.com/s/s4g2uliz0uqdjk3/Unity-install.png?raw=1)

### Blender Install
1. Open the script `MapReportTool.py` in your blender file and click `Run Script`.
	![Blender Script Widnow](https://www.dropbox.com/s/ymfa632wos4rirb/Blender-run.png?raw=1)
	
2. Open 3D View Properties Panel `N-Key` and enter the file to read from.
	![Blender MapTool Panel](https://www.dropbox.com/s/w1tbwyhtyob5bq9/Blender-config.png?raw=1)


