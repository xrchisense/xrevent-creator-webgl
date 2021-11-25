## Installation (Developer Toolchain)

### Dependencies
- https://github.com/Siccity/GLTFUtility
**Hint:** ArgumentNullException: Value cannot be null in build but not in editor. This is most likely due to shaders being stripped from the build. To fix this, add the GLTFUtility shaders to the Always Included Shaders list in Graphic Settings.





### Unity

1. Download Unity Hub: https://unity.com/de/download
2. Create Unity ID: https://id.unity.com/account/new
3. Install Unity Hub and run.
4. In Unity Hub "Sign In" on the upper right with your Unity ID you just created.
5. Get a (personal) license and activate it by clicking "Activate new license". If the server is unresponsive use "Manual Activation".
6. Click "Installs" on the left menu, then "ADD" at the upper right to add a Unity version.
7. <b>Important:</b> Select Unity 2020.3.22f1 (LTS) and click "Next".
8. Select the following modules to install:
   - WebGL Build Support
9. Click "Next" and agree to license terms and "Done".
10. Get a coffee, as the install will take a while.

## Git 

### Branches

1. 'develop' branch ist für den Aktuellen Stand der Entwicklung. Sollte immer funktionsfähig sein. Wird zum Testen genutzt.
2. 


### Initial download (clone)

1. Use Unity 2020.3.22f1
2. 'git clone'
3. 'git checkout develop'

### Update (pull)
1. 'git pull' im Ordner
2. 'git lfs fetch --all' um sicher zu gehen, dass auch alle lfs daten heruntergeladen werden.
3. 
