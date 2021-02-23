# Unity GL Drawer

Class for drawing simple primitive shapes in game or in editor, which depends on what Unity callback the GLDrawer methods are being called from.

Usage example:

```csharp
public class Foo : MonoBehaviour
{
    private void Start()
    {
        RenderPipelineManager.endCameraRendering += AtPostRender;
    }
    
    private void AtPostRender(ScriptableRenderContext scriptableRenderContext, Camera camera1)
    {
        GLDrawer.WireSphere(transform.position,1f,Color.yellow);
    }
}
```
