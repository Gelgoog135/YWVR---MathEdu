#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;
#endif

namespace YWVR.Triangle
{

    public class FreeformTrianglePrimitive
    {
#if UNITY_EDITOR
        private static Mesh GetMesh()
        {
            Vector3[] vertices = {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(0f, 0.5f, 0)
        };

            Vector2[] uv = {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0.5f, 1)
        };

            int[] triangles = { 0, 1, 2 };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }

        private static void CreateLine(GameObject parent, string name)
        {
            var gLine = new GameObject(name);
            gLine.transform.parent = parent.transform;
            var line = gLine.AddComponent<LineRenderer>();
            line.widthMultiplier = 0.02f;
            line.SetPosition(0, new Vector3(0, 0, 0));
            line.SetPosition(1, new Vector3(0, 1, 0));
            line.numCapVertices = 90;
            line.useWorldSpace = false;
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = Color.cyan;
            line.material = mat;
        }

        private static void CreateLabelLine(GameObject parent, string name, string text)
        {
            var gLabelLine = new GameObject(name);
            gLabelLine.transform.parent = parent.transform;
            var labelLine = gLabelLine.AddComponent<TextMeshPro>();
            labelLine.SetText(text);
            labelLine.fontSize = 15.0f;
            labelLine.autoSizeTextContainer = true;
        }

        private static void CreateLabelAngle(GameObject parent, string name, string text)
        {
            var gLabelAngle = new GameObject(name);
            gLabelAngle.transform.parent = parent.transform;
            var labelAngle = gLabelAngle.AddComponent<TextMeshPro>();
            labelAngle.SetText(text);
            labelAngle.fontSize = 15.0f;
            labelAngle.autoSizeTextContainer = true;
        }

        private static GameObject CreateObject()
        {
            //Material matTri = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            Material matTri = Resources.Load("Images/Triangle_Angles", typeof(Material)) as Material;
            //var objs = Resources.LoadAll("Images", typeof(Material));
            //foreach(var obj in objs)
            //{
            //    Debug.Log(obj.name);
            //}
            //matTri.color = Color.gray;


            var container = new GameObject("Freeform Triangle");
            container.AddComponent<FreeformTriangleController>();

            var triXY = new GameObject("Triangle Rotatable XY");
            triXY.transform.parent = container.transform;

            var tri = new GameObject("Triangle Rotatable Z");
            tri.transform.parent = triXY.transform;

            var objF = new GameObject("Triangle Front");
            objF.transform.parent = tri.transform;
            var mesh = GetMesh();
            var filterF = objF.AddComponent<MeshFilter>();
            var rendererF = objF.AddComponent<MeshRenderer>();
            var colliderF = objF.AddComponent<MeshCollider>();

            filterF.sharedMesh = mesh;
            colliderF.sharedMesh = mesh;

            rendererF.material = matTri;


            var objB = new GameObject("Triangle Back");
            objB.transform.parent = tri.transform;
            //objB.transform.rotation *= Quaternion.Euler(0, 180f, 0);
            var filterB = objB.AddComponent<MeshFilter>();
            var rendererB = objB.AddComponent<MeshRenderer>();
            var colliderB = objB.AddComponent<MeshCollider>();
            //objB.AddComponent<FreeformTriangleController>();

            filterB.sharedMesh = mesh;
            colliderB.sharedMesh = mesh;

            rendererB.material = matTri;


            //var gLine1 = new GameObject("Line A");
            //gLine1.transform.parent = tri.transform;
            //var line1 = gLine1.AddComponent<LineRenderer>();
            //line1.widthMultiplier = 0.02f;
            //line1.SetPosition(0, new Vector3(0, 0, 0));
            //line1.SetPosition(1, new Vector3(0, 1, 0));
            //line1.numCapVertices = 90;
            //line1.useWorldSpace = false;
            //Material mat1 = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            //mat1.color = Color.cyan;
            //line1.material = mat1;

            //var gLine2 = new GameObject("Line B");
            //gLine2.transform.parent = tri.transform;
            //var line2 = gLine2.AddComponent<LineRenderer>();
            //line2.widthMultiplier = 0.02f;
            //line2.SetPosition(0, new Vector3(0, 0, 0));
            //line2.SetPosition(1, new Vector3(-1, 0, 0));
            //line2.numCapVertices = 90;
            //line2.useWorldSpace = false;
            //Material mat2 = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            //mat2.color = Color.cyan;
            //line2.material = mat2;

            //var gLine3 = new GameObject("Line C");
            //gLine3.transform.parent = tri.transform;
            //var line3 = gLine3.AddComponent<LineRenderer>();
            //line3.widthMultiplier = 0.02f;
            //line3.SetPosition(0, new Vector3(-1, 0, 0));
            //line3.SetPosition(1, new Vector3(0, 1, 0));
            //line3.numCapVertices = 90;
            //line3.useWorldSpace = false;
            //Material mat3 = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            //mat3.color = Color.cyan;
            //line3.material = mat3;

            CreateLine(tri, "Line A");
            CreateLine(tri, "Line B");
            CreateLine(tri, "Line C");

            CreateLabelLine(tri, "Label Line A", "a");
            CreateLabelLine(tri, "Label Line B", "b");
            CreateLabelLine(tri, "Label Line C", "c");

            CreateLabelAngle(tri, "Label Angle A", "θ");
            CreateLabelAngle(tri, "Label Angle B", "θ");
            CreateLabelAngle(tri, "Label Angle C", "90°");

            var gQues = new GameObject("Question");
            gQues.transform.parent = triXY.transform;
            var txtQuestion = gQues.AddComponent<TextMeshPro>();
            txtQuestion.SetText("Question");
            txtQuestion.fontSize = 10.0f;
            txtQuestion.color = Color.black;
            txtQuestion.autoSizeTextContainer = true;

            //var gLabelLine1 = new GameObject("Label Line A");
            //gLabelLine1.transform.parent = tri.transform;
            //var labelLine1 = gLabelLine1.AddComponent<TextMeshPro>();
            //labelLine1.SetText("a");
            //labelLine1.fontSize = 3.0f;
            //labelLine1.autoSizeTextContainer = true;

            container.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            return container;
        }



        [MenuItem("GameObject/3D Object/Freeform Triangle", false, 0)]
        public static void Create()
        {
            CreateObject();
        }
#endif
    }
}