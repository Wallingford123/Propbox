using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainScript : MonoBehaviour
{
    [HideInInspector]
    public Quat q;
    [HideInInspector]
    public float yawAng, pitchAng, rollAng;
    float lastyawAng, lastpitchAng, lastrollAng;
    public float rotSpeed;
    public Camera cam;
    public float distanceAhead;
    public float lookSpeed;
    public float moveSpeed;
    public float collisionDistance;
    public float bounceDistance;
    public float castWidth;
    public bool isDemo;
    [HideInInspector]
    public Vector3 pos;
    [HideInInspector]
    public float scaleMul;
    public bool isPicked;
    [HideInInspector]
    public bool isKey;
    public bool isFake;
    [HideInInspector]
    public bool notSet;
    [HideInInspector]
    public float g = 0;
    [HideInInspector]
    public float y = 0;
    [HideInInspector]
    public bool placed;
    Matrix4by4 camMat;

    Vector3[] ModelSpaceVertices;
    //Custom matrix class
    public class Matrix4by4
    {
        public Matrix4by4(Vector4 col1, Vector4 col2, Vector4 col3, Vector4 col4)
        {
            values = new float[4, 4];

            values[0, 0] = col1.x;
            values[1, 0] = col1.y;
            values[2, 0] = col1.z;
            values[3, 0] = col1.w;

            values[0, 1] = col2.x;
            values[1, 1] = col2.y;
            values[2, 1] = col2.z;
            values[3, 1] = col2.w;

            values[0, 2] = col3.x;
            values[1, 2] = col3.y;
            values[2, 2] = col3.z;
            values[3, 2] = col3.w;

            values[0, 3] = col4.x;
            values[1, 3] = col4.y;
            values[2, 3] = col4.z;
            values[3, 3] = col4.w;

        }
        public Matrix4by4(Vector3 col1, Vector3 col2, Vector3 col3, Vector3 col4)
        {
            values = new float[4, 4];

            values[0, 0] = col1.x;
            values[1, 0] = col1.y;
            values[2, 0] = col1.z;
            values[3, 0] = 0;

            values[0, 1] = col2.x;
            values[1, 1] = col2.y;
            values[2, 1] = col2.z;
            values[3, 1] = 0;

            values[0, 2] = col3.x;
            values[1, 2] = col3.y;
            values[2, 2] = col3.z;
            values[3, 2] = 0;

            values[0, 3] = col4.x;
            values[1, 3] = col4.y;
            values[2, 3] = col4.z;
            values[3, 3] = 1;
        }
        public float[,] values;
        public static Vector4 operator *(Matrix4by4 lhs, Vector4 vector)
        {
            Vector4 tempVec;
            tempVec.x = ((vector.x * lhs.values[0, 0]) + (vector.y * lhs.values[0, 1]) + (vector.z * lhs.values[0, 2]) + (vector.w * lhs.values[0, 3]));
            tempVec.y = ((vector.x * lhs.values[1, 0]) + (vector.y * lhs.values[1, 1]) + (vector.z * lhs.values[1, 2]) + (vector.w * lhs.values[1, 3]));
            tempVec.z = ((vector.x * lhs.values[2, 0]) + (vector.y * lhs.values[2, 1]) + (vector.z * lhs.values[2, 2]) + (vector.w * lhs.values[2, 3]));
            tempVec.w = ((vector.x * lhs.values[3, 0]) + (vector.y * lhs.values[3, 1]) + (vector.z * lhs.values[3, 2]) + (vector.w * lhs.values[3, 3]));
            return tempVec;
        }

        public static Matrix4by4 operator *(Matrix4by4 lhs, Matrix4by4 rhs)
        {
            Matrix4by4 tempMatrix = new Matrix4by4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
            tempMatrix.values[0, 0] = ((rhs.values[0, 0] * lhs.values[0, 0]) + (rhs.values[1, 0] * lhs.values[0, 1]) + (rhs.values[2, 0] * lhs.values[0, 2]) + (rhs.values[3, 0] * lhs.values[0, 3]));
            tempMatrix.values[0, 1] = ((rhs.values[0, 1] * lhs.values[0, 0]) + (rhs.values[1, 1] * lhs.values[0, 1]) + (rhs.values[2, 1] * lhs.values[0, 2]) + (rhs.values[3, 1] * lhs.values[0, 3]));
            tempMatrix.values[0, 2] = ((rhs.values[0, 2] * lhs.values[0, 0]) + (rhs.values[1, 2] * lhs.values[0, 1]) + (rhs.values[2, 2] * lhs.values[0, 2]) + (rhs.values[3, 2] * lhs.values[0, 3]));
            tempMatrix.values[0, 3] = ((rhs.values[0, 3] * lhs.values[0, 0]) + (rhs.values[1, 3] * lhs.values[0, 1]) + (rhs.values[2, 3] * lhs.values[0, 2]) + (rhs.values[3, 3] * lhs.values[0, 3]));

            tempMatrix.values[1, 0] = ((rhs.values[0, 0] * lhs.values[1, 0]) + (rhs.values[1, 0] * lhs.values[1, 1]) + (rhs.values[2, 0] * lhs.values[1, 2]) + (rhs.values[3, 0] * lhs.values[1, 3]));
            tempMatrix.values[1, 1] = ((rhs.values[0, 1] * lhs.values[1, 0]) + (rhs.values[1, 1] * lhs.values[1, 1]) + (rhs.values[2, 1] * lhs.values[1, 2]) + (rhs.values[3, 1] * lhs.values[1, 3]));
            tempMatrix.values[1, 2] = ((rhs.values[0, 2] * lhs.values[1, 0]) + (rhs.values[1, 2] * lhs.values[1, 1]) + (rhs.values[2, 2] * lhs.values[1, 2]) + (rhs.values[3, 2] * lhs.values[1, 3]));
            tempMatrix.values[1, 3] = ((rhs.values[0, 3] * lhs.values[1, 0]) + (rhs.values[1, 3] * lhs.values[1, 1]) + (rhs.values[2, 3] * lhs.values[1, 2]) + (rhs.values[3, 3] * lhs.values[1, 3]));

            tempMatrix.values[2, 0] = ((rhs.values[0, 0] * lhs.values[2, 0]) + (rhs.values[1, 0] * lhs.values[2, 1]) + (rhs.values[2, 0] * lhs.values[2, 2]) + (rhs.values[3, 0] * lhs.values[2, 3]));
            tempMatrix.values[2, 1] = ((rhs.values[0, 1] * lhs.values[2, 0]) + (rhs.values[1, 1] * lhs.values[2, 1]) + (rhs.values[2, 1] * lhs.values[2, 2]) + (rhs.values[3, 1] * lhs.values[2, 3]));
            tempMatrix.values[2, 2] = ((rhs.values[0, 2] * lhs.values[2, 0]) + (rhs.values[1, 2] * lhs.values[2, 1]) + (rhs.values[2, 2] * lhs.values[2, 2]) + (rhs.values[3, 2] * lhs.values[2, 3]));
            tempMatrix.values[2, 3] = ((rhs.values[0, 3] * lhs.values[2, 0]) + (rhs.values[1, 3] * lhs.values[2, 1]) + (rhs.values[2, 3] * lhs.values[2, 2]) + (rhs.values[3, 3] * lhs.values[2, 3]));

            tempMatrix.values[3, 0] = ((rhs.values[0, 0] * lhs.values[3, 0]) + (rhs.values[1, 0] * lhs.values[3, 1]) + (rhs.values[2, 0] * lhs.values[3, 2]) + (rhs.values[3, 0] * lhs.values[3, 3]));
            tempMatrix.values[3, 1] = ((rhs.values[0, 1] * lhs.values[3, 0]) + (rhs.values[1, 1] * lhs.values[3, 1]) + (rhs.values[2, 1] * lhs.values[3, 2]) + (rhs.values[3, 1] * lhs.values[3, 3]));
            tempMatrix.values[3, 2] = ((rhs.values[0, 2] * lhs.values[3, 0]) + (rhs.values[1, 2] * lhs.values[3, 1]) + (rhs.values[2, 2] * lhs.values[3, 2]) + (rhs.values[3, 2] * lhs.values[3, 3]));
            tempMatrix.values[3, 3] = ((rhs.values[0, 3] * lhs.values[3, 0]) + (rhs.values[1, 3] * lhs.values[3, 1]) + (rhs.values[2, 3] * lhs.values[3, 2]) + (rhs.values[3, 3] * lhs.values[3, 3]));
            return tempMatrix;
        }
    }
    //Custom quaternion class
    public class Quat
    {
        public float w, x, y, z;

        public Quat(float Angle, Vector3 Axis)
        {
            float halfAngle = Angle / 2;
            w = Mathf.Cos(halfAngle);
            x = Axis.x * Mathf.Sin(halfAngle);
            y = Axis.y * Mathf.Sin(halfAngle);
            z = Axis.z * Mathf.Sin(halfAngle);
        }
        public Quat()
        {

        }
        public Quat(Vector3 Axis)
        {
            x = Axis.x;
            y = Axis.y;
            z = Axis.z;
        }
        public Quat Inverse()
        {
            Quat rv = new Quat();

            rv.w = w;
            rv.SetAxis(-GetAxis());

            return rv;
        }
        void SetAxis(Vector3 axis)
        {
            x = axis.x;
            y = axis.y;
            z = axis.z;
        }
        public Vector3 GetAxis()
        {
            Vector3 ax;
            ax.x = x;
            ax.y = y;
            ax.z = z;
            return ax;
        }

        public static Quat operator *(Quat R, Quat S)
        {
            Quat rs = new Quat()
            {
                x = R.x * S.w + R.y * S.z - R.z * S.y + R.w * S.x,
                y = -R.x * S.z + R.y * S.w + R.z * S.x + R.w * S.y,
                z = R.x * S.y - R.y * S.x + R.z * S.w + R.w * S.z,
                w = -R.x * S.x - R.y * S.y - R.z * S.z + R.w * S.w
            };

            return rs;
        }
    }

    void Start()
    {
        camMat = Identity();
        q = new Quat(1, new Vector3(0, 0, 0));
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (!isFake)
        {
            MeshFilter MF = GetComponent<MeshFilter>();
            ModelSpaceVertices = MF.mesh.vertices;
        }
        placed = false;
    }

    void Update()
    {
        //tests if the current instance is being held, or its initial transformations(at level start) have not been set;
        if (isPicked || notSet)
        {
            if (isPicked && pos.y != 0) pos.y = 0;
            if (Input.GetKeyDown(KeyCode.Escape) && Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !Cursor.visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            //tests if the code is only being run to set the initial transformations;
            if (!notSet)
            {
                updatePos();
                testCollision();
                setCam();
                cam.transform.position = new Vector3(pos.x, 0, pos.z);
            }
            //tests if there is an object in hand, and it isn't already placed in the goal location;
            if (!isFake && !placed)
            {
                transformModel();
            }
        }
        //tests if this instance is the demo object, in which case this will run (note: it will never be in a state of 'isPicked', so the preceeding code will never run for the demo object);
        if (isDemo)
        {
            transformDemo();
        }
        //ensures setting of object only occurs once;
        notSet = false;
    }

    //function to return an identity matrix;
    Matrix4by4 Identity()
    {
        Matrix4by4 identity = new Matrix4by4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
        return identity;
    }

    //function to return a translation matrix;
    Matrix4by4 translate(Vector3 trans)
    {
        Matrix4by4 tMat = Identity();
        tMat.values[0, 3] = trans.x;
        tMat.values[1, 3] = trans.y;
        tMat.values[2, 3] = trans.z;
        return tMat;
    }

    //function to return a pitch rotation matrix;
    Matrix4by4 pitchRot(float ang)
    {
        Matrix4by4 mat = 
            new Matrix4by4(new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(y), Mathf.Sin(y)),
            new Vector3(0, -Mathf.Sin(y), Mathf.Cos(y)),
            Vector3.zero);
        return mat;
    }

    //function to return a yaw rotation matrix;
    Matrix4by4 yawRot(float ang)
    {
        Matrix4by4 mat = 
            new Matrix4by4(new Vector3(Mathf.Cos(g), 0, -Mathf.Sin(g)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(g), 0, Mathf.Cos(g)),
            Vector3.zero);
        return mat;
    }
    //function to return a scale matrix;
    Matrix4by4 scale(float sca)
    {
        Matrix4by4 sMat = Identity();
        sMat.values[0, 0] *= sca;
        sMat.values[1, 1] *= sca;
        sMat.values[2, 2] *= sca;
        return sMat;
    }

    //function to get the user's mouse input and updates the rotation angles from it (Also has inputs for scale);
    public void applyMouseInputs()
    {
        if (Input.GetKey(KeyCode.K) && !isKey)
        {
            scaleMul += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.L) && !isKey)
        {
            scaleMul -= Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            rollAng -= (Input.GetAxis("Mouse X") / 100) * rotSpeed;

            //limits value to being a positive value from 0-2*pi for easier debugging;
            if (rollAng > Mathf.PI * 2) rollAng = 0 + (rollAng - Mathf.PI * 2);
            if (rollAng < 0) rollAng = (2 * Mathf.PI) - rollAng;


        }
        if (Input.GetMouseButton(0) || notSet)
        {
            pitchAng += (Input.GetAxis("Mouse Y") / 100) * rotSpeed;

            if (pitchAng > Mathf.PI * 2) pitchAng = 0 + (pitchAng - Mathf.PI * 2);
            if (pitchAng < 0) pitchAng = (2 * Mathf.PI) - pitchAng;


            yawAng -= (Input.GetAxis("Mouse X") / 100) * rotSpeed;

            if (yawAng > Mathf.PI * 2) yawAng = 0 + (yawAng - Mathf.PI * 2);
            if (yawAng < 0) yawAng = (2 * Mathf.PI) - yawAng;


        }
    }

    //function for applying the transformations to the object model
    public void transformModel()
    {
        applyMouseInputs();
        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length];
        //set quaternions to rotate by
        Quat q1 = new Quat(pitchAng - lastpitchAng, new Vector3(1, 0, 0));
        Quat q2 = new Quat(yawAng - lastyawAng, new Vector3(0, 1, 0));
        Quat q3 = new Quat(rollAng - lastrollAng, new Vector3(0, 0, 1));
        //rotate orientation quaternion by pitch yaw and roll quaternion representations;
        q = ((q * q2) * q1) * q3;
        for (int i = 0; i < TransformedVertices.Length; i++)
        {
            //rotate each vertex by the orientation from model space;
            Quat K = new Quat(ModelSpaceVertices[i]);
            Quat newK = q * K * q.Inverse();
            Vector3 newP = newK.GetAxis();
            Vector3 quatVert = newP;

            //scale each vertex by the scale multiplier;
            Matrix4by4 sMatrix = scale(scaleMul);
            Vector3 scaVert = sMatrix * quatVert;

            //translate the object so it will be a set distance ahead of the player;
            Matrix4by4 tMat = translate(new Vector3(0, 0, distanceAhead));
            Vector4 ratVert = new Vector4(scaVert.x, scaVert.y, scaVert.z, 1);
            Vector4 scatVert = tMat * ratVert;

            //rotate the object around the origin(camera), so it is always in front of the camera;
            Matrix4by4 yawMatrix = yawRot(g);
            Matrix4by4 pitchMatrix = pitchRot(y);
            Matrix4by4 pyMatrix = yawMatrix * pitchMatrix;
            Vector3 scarotVert = pyMatrix * scatVert;

            //translate the object so it is correctly positioned in the world;
            Matrix4by4 tMatrix = translate(new Vector3(pos.x, 0, pos.z));
            Vector4 tVert = new Vector4(scarotVert.x, scarotVert.y, scarotVert.z, 1);
            Vector4 finalVert = tMatrix * tVert;
            TransformedVertices[i] = finalVert;
        }
        //set mesh to the transformed vertices
        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices;
        MF.mesh.RecalculateBounds();
        MF.mesh.RecalculateNormals();

        //sets angles so it doesn't accumulate and increase speed over time;
        lastpitchAng = pitchAng;
        lastyawAng = yawAng;
        lastrollAng = rollAng;
    }

    //function for applying transformation to the demo object, rotating around to always face the player to make it easier to work out the correct orientation. functions the same as transformModel();
    public void transformDemo()
    {
        Vector3[] TransformedVertices = new Vector3[ModelSpaceVertices.Length];
        Quat q1 = new Quat(pitchAng - lastpitchAng, new Vector3(1, 0, 0));
        Quat q2 = new Quat(yawAng - lastyawAng, new Vector3(0, 1, 0));
        Quat q3 = new Quat(rollAng - lastrollAng, new Vector3(0, 0, 1));
        q = ((q * q2) * q1) * q3;
        for (int i = 0; i < TransformedVertices.Length; i++)
        {
            Quat K = new Quat(ModelSpaceVertices[i]);
            Quat newK = q * K * q.Inverse();
            Vector3 newP = newK.GetAxis();

            Vector3 quatVert = newP;

            Matrix4by4 sMatrix = scale(scaleMul);
            Vector3 scaVert = sMatrix * quatVert;

            Matrix4by4 yawMatrix = new Matrix4by4(new Vector3(Mathf.Cos(g), 0, -Mathf.Sin(g)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(g), 0, Mathf.Cos(g)),
            Vector3.zero);
            Vector3 scarotVert = yawMatrix * scaVert;

            Matrix4by4 tMatrix = translate(new Vector3(pos.x, 0, pos.z));
            Vector4 tVert = new Vector4(scarotVert.x, scarotVert.y, scarotVert.z, 1);
            Vector4 finalVert = tMatrix * tVert;
            TransformedVertices[i] = finalVert;
        }
        MeshFilter MF = GetComponent<MeshFilter>();
        MF.mesh.vertices = TransformedVertices;
        MF.mesh.RecalculateBounds();
        MF.mesh.RecalculateNormals();

        lastpitchAng = pitchAng;
        lastyawAng = yawAng;
        lastrollAng = rollAng;
    }

    //function to test collision with the maze walls (very gimmicky, definitely not ideal. But serves its purpose);
    public void testCollision()
    {
        if (Physics.Raycast(new Vector3(cam.transform.position.x - castWidth, 0, cam.transform.position.z), new Vector3(0, 0, 1), collisionDistance) ||
        Physics.Raycast(new Vector3(cam.transform.position.x + castWidth, 0, cam.transform.position.z), new Vector3(0, 0, 1), collisionDistance))
        {
            pos.z -= bounceDistance / 100;
        }
        if (Physics.Raycast(new Vector3(cam.transform.position.x - castWidth, 0, cam.transform.position.z), new Vector3(0, 0, -1), collisionDistance) ||
            Physics.Raycast(new Vector3(cam.transform.position.x + castWidth, 0, cam.transform.position.z), new Vector3(0, 0, -1), collisionDistance))
        {
            pos.z += bounceDistance / 100;
        }
        if (Physics.Raycast(new Vector3(cam.transform.position.x, 0, cam.transform.position.z - castWidth), new Vector3(-1, 0, 0), collisionDistance) ||
            Physics.Raycast(new Vector3(cam.transform.position.x, 0, cam.transform.position.z + castWidth), new Vector3(-1, 0, 0), collisionDistance))
        {
            pos.x += bounceDistance / 100;
        }
        if (Physics.Raycast(new Vector3(cam.transform.position.x, 0, cam.transform.position.z - castWidth), new Vector3(1, 0, 0), collisionDistance) ||
            Physics.Raycast(new Vector3(cam.transform.position.x, 0, cam.transform.position.z + castWidth), new Vector3(1, 0, 0), collisionDistance))
        {
            pos.x -= bounceDistance / 100;
        }
    }

    //function to update the position based on user input;
    public void updatePos()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pos += new Vector3(camMat.values[2,0],camMat.values[2,1],camMat.values[2,2]) / 100 * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos -= new Vector3(camMat.values[2, 0], camMat.values[2, 1], camMat.values[2, 2]) / 100 * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos -= new Vector3(camMat.values[0, 0], camMat.values[0, 1], camMat.values[0, 2]) / 100 * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += new Vector3(camMat.values[0, 0], camMat.values[0, 1], camMat.values[0, 2]) / 100 * moveSpeed;
        }
    }

    //function to rotate the camera based on user input;
    public void setCam()
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            g += Input.GetAxis("Mouse X") / 100 * lookSpeed;
            if (g > Mathf.PI * 2) g = 0 + (g - Mathf.PI * 2);
            if (g < 0) g = (2 * Mathf.PI) - g;

            float t = y;
            y -= Input.GetAxis("Mouse Y") / 100 * lookSpeed;
            if (y > Mathf.PI * 2) y = 0 + (y - Mathf.PI * 2);
            if (y < 0) y = (2 * Mathf.PI) - y;
            if (y >= 1.3f && y <= 5.5f) y = t;

            Quat yawQ = new Quat(g, new Vector3(0, 1, 0));
            Quat pitchQ = new Quat(y, new Vector3(1, 0, 0));
            Quat pyQ = yawQ * pitchQ;
            camMat = Identity();
            //converts quaternion to matrix to get new basis vectors for forward and right directions(for movement)
            Matrix4by4 mat1 = new Matrix4by4(new Vector4(pyQ.w,-pyQ.z,pyQ.y,-pyQ.x),
                new Vector4(pyQ.z,pyQ.w,-pyQ.x, -pyQ.y),
                new Vector4(-pyQ.y,pyQ.x,pyQ.w,pyQ.z),
                new Vector4(-pyQ.x,-pyQ.y,-pyQ.z,pyQ.w));

            Matrix4by4 mat2 = new Matrix4by4(new Vector4(pyQ.w, -pyQ.z, pyQ.y, pyQ.x),
                new Vector4(pyQ.z, pyQ.w, -pyQ.x, pyQ.y),
                new Vector4(-pyQ.y, pyQ.x, pyQ.w, -pyQ.z),
                new Vector4(pyQ.x, pyQ.y, pyQ.z, pyQ.w));
            camMat = (camMat * mat1) * mat2;
            cam.transform.rotation = new Quaternion(pyQ.x, pyQ.y, pyQ.z, pyQ.w);
        }
    }
}
