using UnityEngine;
using System.Collections;
using TMPro;

namespace YWVR.Triangle
{

    [ExecuteAlways]
    public class FreeformTriangleController : MonoBehaviour
    {
        private Vector3 point1 = new Vector3(0, 0, 0);
        private Vector3 point2 = new Vector3(1.0f, 0, 0);
        private Vector3 point3 = new Vector3(0, 1.0f, 0);

        public float lengthA = 4;
        public float lengthB = 4;
        [ReadOnly] public float lengthC = Mathf.Sqrt(32);

        [ReadOnly] public float angleA = 45;
        [ReadOnly] public float angleB = 45;
        [ReadOnly] public float angleC = 90;

        // Use this for initialization
        void Start()
        {
            Update();
        }

        // Update is called once per frame
        void Update()
        {

            CalculateSidesAndAngles();
            var triXY = transform.Find("Triangle Rotatable XY").gameObject;
            var tri = triXY.transform.Find("Triangle Rotatable Z").gameObject;
            //Debug.Log('j');
            var front = tri.transform.Find("Triangle Front").gameObject;

            var filterF = front.GetComponent<MeshFilter>();
            var colliderF = front.GetComponent<MeshCollider>();

            var meshF = GetMesh(true);
            filterF.sharedMesh = meshF;
            colliderF.sharedMesh = meshF;

            //var back = tri.transform.Find("Triangle Back").gameObject;

            //var filterB = back.GetComponent<MeshFilter>();
            //var colliderB = back.GetComponent<MeshCollider>();
            //var meshB = GetMesh(false);

            //filterB.sharedMesh = meshB;
            //colliderB.sharedMesh = meshB;

            //var lineA = tri.transform.Find("Line A").gameObject;
            //var lineB = tri.transform.Find("Line B").gameObject;
            //var lineC = tri.transform.Find("Line C").gameObject;

            //var line1 = lineA.GetComponent<LineRenderer>();
            //var line2 = lineB.GetComponent<LineRenderer>();
            //var line3 = lineC.GetComponent<LineRenderer>();

            //line1.SetPosition(0, new Vector3(0, 0, 0));
            //line1.SetPosition(1, new Vector3(0, lengthA, 0));

            //line2.SetPosition(0, new Vector3(0, 0, 0));
            //line2.SetPosition(1, new Vector3(-lengthB, 0, 0));

            //line3.SetPosition(0, new Vector3(-lengthB, 0, 0));
            //line3.SetPosition(1, new Vector3(0, lengthA, 0));

            var labelLineA = tri.transform.Find("Label Line A").gameObject;
            var labelLineB = tri.transform.Find("Label Line B").gameObject;
            var labelLineC = tri.transform.Find("Label Line C").gameObject;

            labelLineA.transform.localPosition = new Vector3(1f, lengthA / 2);
            labelLineB.transform.localPosition = new Vector3(-lengthB / 2, -1f);
            labelLineC.transform.localPosition = new Vector3(-lengthB / 2 - 0.707f, lengthA / 2 + 0.707f);

            labelLineA.transform.rotation = Quaternion.Euler(labelLineA.transform.eulerAngles.x, labelLineA.transform.eulerAngles.y, 0);
            labelLineB.transform.rotation = Quaternion.Euler(labelLineB.transform.eulerAngles.x, labelLineB.transform.eulerAngles.y, 0);
            labelLineC.transform.rotation = Quaternion.Euler(labelLineC.transform.eulerAngles.x, labelLineC.transform.eulerAngles.y, 0);


            var labelAngleA = tri.transform.Find("Label Angle A").gameObject;
            var labelAngleB = tri.transform.Find("Label Angle B").gameObject;
            var labelAngleC = tri.transform.Find("Label Angle C").gameObject;

            labelAngleA.transform.localPosition = new Vector3(-lengthB - 0.707f, -0.707f);
            labelAngleB.transform.localPosition = new Vector3(0.707f, lengthA + 0.707f);
            labelAngleC.transform.localPosition = new Vector3(0.707f, -0.707f);

            labelAngleA.transform.rotation = Quaternion.Euler(labelAngleA.transform.eulerAngles.x, labelAngleA.transform.eulerAngles.y, 0);
            labelAngleB.transform.rotation = Quaternion.Euler(labelAngleB.transform.eulerAngles.x, labelAngleB.transform.eulerAngles.y, 0);
            labelAngleC.transform.rotation = Quaternion.Euler(labelAngleC.transform.eulerAngles.x, labelAngleC.transform.eulerAngles.y, 0);

            var txtQuestion = triXY.transform.Find("Question").gameObject;
            //txtQuestion.transform.localPosition = new Vector3(re.bounds.size.x / 2, re.bounds.size.y / 2);
            Vector3 newPos = (labelLineA.transform.position + labelLineB.transform.position + labelLineC.transform.position) / 3;
            txtQuestion.transform.position = newPos;

            txtQuestion.transform.localPosition = new Vector3(txtQuestion.transform.localPosition.x, txtQuestion.transform.localPosition.y, txtQuestion.transform.localPosition.z - 0.5f);
            //Debug.Log(labelLineA.transform.position);
        }

        public void SetQuestion(Question q, float lengthA, float lengthB, float rotationY, float rotationZ)
        {
            SetLengths(lengthA, lengthB);
            var triXY = transform.Find("Triangle Rotatable XY").gameObject;
            var tri = triXY.transform.Find("Triangle Rotatable Z").gameObject;


            triXY.transform.rotation = Quaternion.Euler(triXY.transform.eulerAngles.x, triXY.transform.eulerAngles.y, triXY.transform.eulerAngles.z);
            tri.transform.rotation = Quaternion.Euler(tri.transform.eulerAngles.x, tri.transform.eulerAngles.y, rotationZ);

            var gLabelLineA = tri.transform.Find("Label Line A").gameObject;
            var gLabelLineB = tri.transform.Find("Label Line B").gameObject;
            var gLabelLineC = tri.transform.Find("Label Line C").gameObject;

            var txtLineA = gLabelLineA.GetComponent<TextMeshPro>();
            var txtLineB = gLabelLineB.GetComponent<TextMeshPro>();
            var txtLineC = gLabelLineC.GetComponent<TextMeshPro>();

            var gLabelAngleA = tri.transform.Find("Label Angle A").gameObject;
            var gLabelAngleB = tri.transform.Find("Label Angle B").gameObject;
            var gLabelAngleC = tri.transform.Find("Label Angle C").gameObject;

            var txtAngleA = gLabelAngleA.GetComponent<TextMeshPro>();
            var txtAngleB = gLabelAngleB.GetComponent<TextMeshPro>();
            var txtAngleC = gLabelAngleC.GetComponent<TextMeshPro>();

            var gQuestion = triXY.transform.Find("Question").gameObject;
            var txtQuestion = gQuestion.GetComponent<TextMeshPro>();

            txtLineA.text = q.LineA;
            txtLineB.text = q.LineB;
            txtLineC.text = q.LineC;
            txtAngleA.text = q.AngleA;
            txtAngleB.text = q.AngleB;
            txtAngleC.text = q.AngleC;
            txtQuestion.text = q.Expression;

            txtLineA.autoSizeTextContainer = false;
            txtLineB.autoSizeTextContainer = false;
            txtLineC.autoSizeTextContainer = false;
            txtAngleA.autoSizeTextContainer = false;
            txtAngleB.autoSizeTextContainer = false;
            txtAngleC.autoSizeTextContainer = false;
            txtQuestion.autoSizeTextContainer = false;

            txtLineA.autoSizeTextContainer = true;
            txtLineB.autoSizeTextContainer = true;
            txtLineC.autoSizeTextContainer = true;
            txtAngleA.autoSizeTextContainer = true;
            txtAngleB.autoSizeTextContainer = true;
            txtAngleC.autoSizeTextContainer = true;
            txtQuestion.autoSizeTextContainer = true;
        }

        public void SetLengths(float a, float b)
        {
            lengthA = a;
            lengthB = b;
            Update();
        }

        private void CalculateSidesAndAngles()
        {
            lengthC = Mathf.Sqrt(lengthA * lengthA + lengthB * lengthB);
            angleA = Mathf.Rad2Deg * Mathf.Asin(lengthA / lengthC);
            angleB = Mathf.Rad2Deg * Mathf.Asin(lengthB / lengthC);
            angleC = 90;
        }

        private Mesh GetMesh(bool isFront)
        {
            //Vector3[] vertices = {
            //    point1, point2, point3
            //};

            Vector3[] vertices = new Vector3[]{
            new Vector3(0, 0, 0),
            new Vector3(-lengthB, 0, 0),
            new Vector3(0, lengthA, 0)
        };
            int[] triangles;

            if (isFront)
            {
                triangles = new int[] { 0, 1, 2 };
            }
            else
            {
                triangles = new int[] { 2, 1, 0 };
            }

            Vector2[] uv = {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1)
        };

            //Vector2[] uv = new Vector2[vertices.Length];
            //for (int i = 0; i < uv.Length; i++)
            //{
            //    uv[i] = new Vector2(vertices[i].x, vertices[i].z);
            //}


            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }
    }

}