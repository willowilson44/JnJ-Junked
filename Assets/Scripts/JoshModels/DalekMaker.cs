using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DalekMaker : MonoBehaviour
{

    private int detail;

    //Base
    private GameObject Base;
    private GameObject BaseUpper;
    private GameObject[] BaseBalls;
    private GameObject[] BaseBalls1;
    private GameObject[] BaseBalls2;
    private GameObject[] BaseBalls3;
    private GameObject[] BaseBalls4;
    private GameObject[] BaseBalls5;
    private GameObject[] BaseBalls6;
    private GameObject[] BaseBalls7;

    //Middle
    private GameObject Middle;
    private GameObject MiddleUpperRing;
    private GameObject MiddleBoxRight;
    private GameObject MiddleBoxLeft;
    private GameObject LeftProdderRotationJoint;
    private GameObject RightProdderRotationJoint;
    private GameObject LeftProdderBall;
    private GameObject RightProdderBall;
    private GameObject LeftWristJoint;
    private GameObject LeftWrist;
    private GameObject LeftHand;
    private GameObject LeftProdder;
    private GameObject LeftProdderTip;
    private GameObject RightProdder;
    private GameObject RightProdderTip;

    //Head
    private GameObject Head;
    private GameObject HeadRing1;
    private GameObject HeadRing2;
    private GameObject HeadRing3;
    private GameObject HeadColumns;
    private GameObject CapRotationJoint;
    private GameObject CapBottom;
    private GameObject CapTop;
    private GameObject GunRotationJoint;
    private GameObject GunWheel;
    private GameObject GunBarrel;
    private GameObject GunSpring;
    private GameObject GunMuzzel;
    private GameObject GunTip;
    private GameObject GunEye;


    public Material BaseColor;
    public Material MiddleColor;
    public Material HeadTubeColor;
    public Material BallColor;
    public Material ChromeColor;


    // Start is called before the first frame update
    void Start()
    {
        detail = 20;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        //---------Base-----------

        //Base Profile
        Vector3[] baseProfile = JoshUtilities.MakeOvalProfile(0.8f, 1.1f, 12);

        //Base Path
        Matrix4x4[] basePath = new Matrix4x4[8];
        basePath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        basePath[1] = Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        basePath[2] = Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        basePath[3] = Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        basePath[4] = Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.2f)); // Top
        basePath[5] = Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.2f)); // Top
        basePath[6] = Matrix4x4.Scale(new Vector3(1.1f, 1.1f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.2f)); // Top
        basePath[7] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.2f)); // Top

        //Base
        Base = new GameObject();
        Base.name = "Base";
        MeshRenderer meshRenderer = Base.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        MeshFilter meshFilter = Base.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(baseProfile, basePath, false);
        Base.transform.parent = transform;
        Base.transform.localPosition = new Vector3(0, 0, 0);
        Base.transform.localRotation = Quaternion.Euler(new Vector3(-90, 180, 0));

        //BaseUpper Path
        Matrix4x4[] baseUpperPath = new Matrix4x4[8];
        baseUpperPath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        baseUpperPath[1] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        baseUpperPath[2] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        baseUpperPath[3] = Matrix4x4.Scale(new Vector3(1, 1, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        baseUpperPath[4] = Matrix4x4.Scale(new Vector3(0.72f, 0.66f, 1)) * Matrix4x4.Translate(new Vector3(0, -0.2f, 1.3f)); // Top
        baseUpperPath[5] = Matrix4x4.Scale(new Vector3(0.72f, 0.66f, 1)) * Matrix4x4.Translate(new Vector3(0, -0.2f, 1.3f)); // Top
        baseUpperPath[6] = Matrix4x4.Scale(new Vector3(0.72f, 0.66f, 1)) * Matrix4x4.Translate(new Vector3(0, -0.2f, 1.3f)); // Top
        baseUpperPath[7] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, -0.24f, 1.3f)); // Top

        //BaseUpper
        BaseUpper = new GameObject();
        BaseUpper.name = "BaseUpper";
        meshRenderer = BaseUpper.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = BaseUpper.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(baseProfile, baseUpperPath, false);
        BaseUpper.transform.parent = Base.transform;
        BaseUpper.transform.localPosition = new Vector3(0, 0, 0.2f);
        BaseUpper.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //BaseBalls
        BaseBalls = new GameObject[4];

        for(int i=0; i < 4; i++)
        {
            BaseBalls[i] = new GameObject();
            BaseBalls[i].name = "BaseBalls";
            meshRenderer = BaseBalls[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls[i].transform.parent = BaseUpper.transform;
            BaseBalls[i].transform.localPosition = new Vector3(0.2f-(i*0.0133f), 0.95f-(0.11f*i), 0.2f + (i * 0.3f));
            BaseBalls[i].transform.localRotation = Quaternion.Euler(new Vector3(-61, 41.7f, 0));
        }

        //BaseBalls 1
        BaseBalls1 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls1[i] = new GameObject();
            BaseBalls1[i].name = "BaseBalls1";
            meshRenderer = BaseBalls1[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls1[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls1[i].transform.parent = BaseUpper.transform;
            BaseBalls1[i].transform.localPosition = new Vector3(-0.2f + (i * 0.0133f), 0.95f - (0.11f * i), 0.2f + (i * 0.3f));
            BaseBalls1[i].transform.localRotation = Quaternion.Euler(new Vector3(-61, -41.7f, 0));
        }

        //BaseBalls 2
        BaseBalls2 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls2[i] = new GameObject();
            BaseBalls2[i].name = "BaseBalls2";
            meshRenderer = BaseBalls2[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls2[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls2[i].transform.parent = BaseUpper.transform;
            BaseBalls2[i].transform.localPosition = new Vector3(0.509f - (i * 0.033f), 0.705f - (0.095f * i), 0.2f + (i * 0.3f));
            BaseBalls2[i].transform.localRotation = Quaternion.Euler(new Vector3(-34.7f, 72.1f, 0));
        }

        //BaseBalls 3
        BaseBalls3 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls3[i] = new GameObject();
            BaseBalls3[i].name = "BaseBalls3";
            meshRenderer = BaseBalls3[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls3[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls3[i].transform.parent = BaseUpper.transform;
            BaseBalls3[i].transform.localPosition = new Vector3(-0.509f + (i * 0.033f), 0.705f - (0.095f * i), 0.2f + (i * 0.3f));
            BaseBalls3[i].transform.localRotation = Quaternion.Euler(new Vector3(-34.7f, -72.1f, 0));
        }

        //BaseBalls 4
        BaseBalls4 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls4[i] = new GameObject();
            BaseBalls4[i].name = "BaseBalls4";
            meshRenderer = BaseBalls4[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls4[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls4[i].transform.parent = BaseUpper.transform;
            BaseBalls4[i].transform.localPosition = new Vector3(0.7133f - (i * 0.048f), 0.245f - (0.054f * i), 0.2f + (i * 0.3f));
            BaseBalls4[i].transform.localRotation = Quaternion.Euler(new Vector3(-10.4f, 78.7f, 0));
        }

        //BaseBalls 5
        BaseBalls5 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls5[i] = new GameObject();
            BaseBalls5[i].name = "BaseBalls5";
            meshRenderer = BaseBalls5[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls5[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls5[i].transform.parent = BaseUpper.transform;
            BaseBalls5[i].transform.localPosition = new Vector3(-0.7133f + (i * 0.048f), 0.245f - (0.054f * i), 0.2f + (i * 0.3f));
            BaseBalls5[i].transform.localRotation = Quaternion.Euler(new Vector3(-10.4f, -78.7f, 0));
        }

        //BaseBalls 6
        BaseBalls6 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls6[i] = new GameObject();
            BaseBalls6[i].name = "BaseBalls6";
            meshRenderer = BaseBalls6[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls6[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls6[i].transform.parent = BaseUpper.transform;
            BaseBalls6[i].transform.localPosition = new Vector3(0.7125f - (i * 0.04683f), -0.281f - (0.007f * i), 0.2f + (i * 0.3f));
            BaseBalls6[i].transform.localRotation = Quaternion.Euler(new Vector3(9.12f, 82.06f, 0));
        }

        //BaseBalls 7
        BaseBalls7 = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            BaseBalls7[i] = new GameObject();
            BaseBalls7[i].name = "BaseBalls7";
            meshRenderer = BaseBalls7[i].AddComponent<MeshRenderer>();
            if (BallColor == null)
            {
                BallColor = new Material(Shader.Find("Standard"));
            }
            meshRenderer.sharedMaterial = BallColor;
            meshFilter = BaseBalls7[i].AddComponent<MeshFilter>();
            meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1.3f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.13f, 0.5f), false);
            BaseBalls7[i].transform.parent = BaseUpper.transform;
            BaseBalls7[i].transform.localPosition = new Vector3(-0.7125f + (i * 0.04683f), -0.281f - (0.007f * i), 0.2f + (i * 0.3f));
            BaseBalls7[i].transform.localRotation = Quaternion.Euler(new Vector3(9.12f, -82.06f, 0));
        }

        //----------------------------Middle-----------------------------------

        //Middle Path
        Matrix4x4[] MiddlePath = new Matrix4x4[6];
        MiddlePath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        MiddlePath[1] = Matrix4x4.Scale(new Vector3(0.72f, 0.66f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        MiddlePath[2] = Matrix4x4.Scale(new Vector3(0.72f, 0.66f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        MiddlePath[3] = Matrix4x4.Scale(new Vector3(0.66f, 0.52f, 1)) * Matrix4x4.Translate(new Vector3(0, -0.07f, 0.65f)); // Top
        MiddlePath[4] = Matrix4x4.Scale(new Vector3(0.66f, 0.52f, 1)) * Matrix4x4.Translate(new Vector3(0, -0.07f, 0.65f)); // Top
        MiddlePath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, -0.07f, 0.65f)); // Top

        //Middle
        Middle = new GameObject();
        Middle.name = "Middle";
        meshRenderer = Middle.AddComponent<MeshRenderer>();
        if (MiddleColor == null)
        {
            MiddleColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = MiddleColor;
        meshFilter = Middle.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(baseProfile, MiddlePath, false);
        Middle.transform.parent = Base.transform;
        Middle.transform.localPosition = new Vector3(0, -0.133f, 1.5f);
        Middle.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //LeftProdderRotationJoint
        LeftProdderRotationJoint = new GameObject();
        LeftProdderRotationJoint.name = "LeftProdderRotationJoint";
        LeftProdderRotationJoint.transform.parent = Middle.transform;
        LeftProdderRotationJoint.transform.localPosition = new Vector3(-0.29f, 0.58f, 0.27f);
        LeftProdderRotationJoint.transform.localRotation = Quaternion.Euler(new Vector3(-270, 0, 0));

        //RightProdderRotationJoint
        RightProdderRotationJoint = new GameObject();
        RightProdderRotationJoint.name = "RightProdderRotationJoint";
        RightProdderRotationJoint.transform.parent = Middle.transform;
        RightProdderRotationJoint.transform.localPosition = new Vector3(0.28f, 0.58f, 0.27f);
        RightProdderRotationJoint.transform.localRotation = Quaternion.Euler(new Vector3(-270, 0, 0));

        //MiddleBoxLeft
        MiddleBoxLeft = new GameObject();
        MiddleBoxLeft.name = "MiddleBoxLeft";
        meshRenderer = MiddleBoxLeft.AddComponent<MeshRenderer>();
        if (MiddleColor == null)
        {
            MiddleColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = MiddleColor;
        meshFilter = MiddleBoxLeft.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cube(0.1f);
        MiddleBoxLeft.transform.parent = Middle.transform;
        MiddleBoxLeft.transform.localPosition = new Vector3(-0.29f, 0.49f, 0.24f);
        MiddleBoxLeft.transform.localRotation = Quaternion.Euler(new Vector3(15f, -0.27f, -0.8f));

        //MiddleBoxRight
        MiddleBoxRight = new GameObject();
        MiddleBoxRight.name = "MiddleBoxLeft";
        meshRenderer = MiddleBoxRight.AddComponent<MeshRenderer>();
        if (MiddleColor == null)
        {
            MiddleColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = MiddleColor;
        meshFilter = MiddleBoxRight.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cube(0.1f);
        MiddleBoxRight.transform.parent = Middle.transform;
        MiddleBoxRight.transform.localPosition = new Vector3(0.28f, 0.49f, 0.24f);
        MiddleBoxRight.transform.localRotation = Quaternion.Euler(new Vector3(15f, -0.27f, -0.8f));

        ////MiddleUpperRing
        //MiddleUpperRing = new GameObject();
        //MiddleUpperRing.name = "MiddleUpperRing";
        //meshRenderer = MiddleUpperRing.AddComponent<MeshRenderer>(); meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        //meshFilter = MiddleUpperRing.AddComponent<MeshFilter>();
        //meshFilter.mesh = MeshUtilities.Cylinder(24, 0.25f, 0.1f);
        //MiddleUpperRing.transform.parent = Middle.transform;
        //MiddleUpperRing.transform.localPosition = new Vector3(0, 0.3f, -0.25f);
        //MiddleUpperRing.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        ////MiddleLowerRing
        //HeadTube = new GameObject();
        //HeadTube.name = "MiddleLowerRing";
        //meshRenderer = HeadTube.AddComponent<MeshRenderer>(); meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        //meshFilter = HeadTube.AddComponent<MeshFilter>();
        //meshFilter.mesh = MeshUtilities.Cylinder(24, 0.08f, 0.01f);
        //HeadTube.transform.parent = Middle.transform;
        //HeadTube.transform.localPosition = new Vector3(0.13f, -0.1f, 0);
        //HeadTube.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //---------------------------------Prodders-----------------------------------

        //LeftProdderBall
        LeftProdderBall = new GameObject();
        LeftProdderBall.name = "LeftProdderBall";
        meshRenderer = LeftProdderBall.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        meshFilter = LeftProdderBall.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1f,detail), JoshUtilities.MakeSpherePath(detail/2,0.12f,0), false);
        LeftProdderBall.transform.parent = LeftProdderRotationJoint.transform;
        LeftProdderBall.transform.localPosition = new Vector3(0f, 0f, 0f);
        LeftProdderBall.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //LeftProdder
        LeftProdder = new GameObject();
        LeftProdder.name = "LeftProdder";
        meshRenderer = LeftProdder.AddComponent<MeshRenderer>();
        if (ChromeColor == null)
        {
            ChromeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = ChromeColor;
        meshFilter = LeftProdder.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cylinder(detail, 0.030f, 0.48f);
        LeftProdder.transform.parent = LeftProdderRotationJoint.transform;
        LeftProdder.transform.localPosition = new Vector3(0, 0.0f, -0.48f);
        LeftProdder.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

        // Shade Sweep profile
        Vector3[] shadeProfile = new Vector3[13];
        shadeProfile[12] = new Vector3(0.0f, -0.05f, 0.0f);
        shadeProfile[11] = new Vector3(0.0f, 0.05f, 0.0f);
        shadeProfile[10] = new Vector3(0.05f, 0.06f, 0.0f);
        shadeProfile[9] = new Vector3(0.05f, 0.06f, 0.0f);
        shadeProfile[8] = new Vector3(0.1f, 0.12f, 0.0f);
        shadeProfile[7] = new Vector3(0.13f, 0.18f, 0.0f);
        shadeProfile[6] = new Vector3(0.13f, 0.18f, 0.0f);
        shadeProfile[5] = new Vector3(0.135f, 0.17f, 0.0f);
        shadeProfile[4] = new Vector3(0.135f, 0.17f, 0.0f);
        shadeProfile[3] = new Vector3(0.11f, 0.11f, 0.0f);
        shadeProfile[2] = new Vector3(0.06f, 0.04f, 0.0f);
        shadeProfile[1] = new Vector3(0.04f, -0.04f, 0.0f);
        shadeProfile[0] = new Vector3(0f, -0.06f, 0.0f);

        //LeftProdderTip
        LeftProdderTip = new GameObject();
        LeftProdderTip.name = "LeftProdderTip";
        meshRenderer = LeftProdderTip.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        meshFilter = LeftProdderTip.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(shadeProfile, MeshUtilities.MakeCirclePath(0.03f,detail), true);
        LeftProdderTip.transform.parent = LeftProdderRotationJoint.transform;
        LeftProdderTip.transform.localPosition = new Vector3(0, 0.0f, -0.95f);
        LeftProdderTip.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        LeftProdderTip.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        //RightProdderBall
        RightProdderBall = new GameObject();
        RightProdderBall.name = "RightProdderBall";
        meshRenderer = RightProdderBall.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        meshFilter = RightProdderBall.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(1f, detail), JoshUtilities.MakeSpherePath(detail / 2, 0.12f, 0), false);
        RightProdderBall.transform.parent = RightProdderRotationJoint.transform;
        RightProdderBall.transform.localPosition = new Vector3(0f, 0f, 0f);
        RightProdderBall.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //RightProdder Path
        Matrix4x4[] RightProdderPath = new Matrix4x4[6];
        RightProdderPath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        RightProdderPath[1] = Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        RightProdderPath[2] = Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        RightProdderPath[3] = Matrix4x4.Scale(new Vector3(0.95f, 0.95f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.7f)); // Bottom
        RightProdderPath[4] = Matrix4x4.Scale(new Vector3(0.95f, 0.95f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.7f)); // Bottom
        RightProdderPath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.7f)); // Bottom

        //RightProdder
        RightProdder = new GameObject();
        RightProdder.name = "RightProdder";
        meshRenderer = RightProdder.AddComponent<MeshRenderer>();
        if (ChromeColor == null)
        {
            ChromeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = ChromeColor;
        meshFilter = RightProdder.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(JoshUtilities.MakeWavyCircleProfile(0.04f, 36, 0.01f, 6f), RightProdderPath, false);
        RightProdder.transform.parent = RightProdderRotationJoint.transform;
        RightProdder.transform.localPosition = new Vector3(0, 0.0f, -0.72f);
        RightProdder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //HeadRing Profile
        Vector3[] ringProfile = new Vector3[4];
        ringProfile[3] = new Vector3(0.08f, 0.03f, 0.0f);
        ringProfile[2] = new Vector3(0.04f, 0, 0.0f);
        ringProfile[1] = new Vector3(0, 0, 0.0f);
        ringProfile[0] = new Vector3(0, 0.03f, 0.0f);

        //RightProdderTip
        RightProdderTip = new GameObject();
        RightProdderTip.name = "RightProdderTip";
        meshRenderer = RightProdderTip.AddComponent<MeshRenderer>();
        if (ChromeColor == null)
        {
            ChromeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = ChromeColor;
        meshFilter = RightProdderTip.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(ringProfile, MeshUtilities.MakeCirclePath(0.03f, detail/2), true);
        RightProdderTip.transform.parent = RightProdderRotationJoint.transform;
        RightProdderTip.transform.localPosition = new Vector3(0, 0.0f, -0.765f);
        RightProdderTip.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        RightProdderTip.transform.localScale = new Vector3(0.25f, 1.5f, 0.25f);



        //-------------------------Head-------------------------

        //HeadTube
        Head = new GameObject();
        Head.name = "Head";
        meshRenderer = Head.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        meshFilter = Head.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cylinder(10, 0.45f, 0.25f);
        Head.transform.parent = Base.transform;
        Head.transform.localPosition = new Vector3(0, -0.16f, 2.40f);
        Head.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

        //CapRotationJoint
        CapRotationJoint = new GameObject();
        CapRotationJoint.name = "CapRotationJoint";
        CapRotationJoint.transform.parent = Head.transform;
        CapRotationJoint.transform.localPosition = new Vector3(0, -0.25f, 0);
        CapRotationJoint.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 180));

        //GunRotationJoint
        GunRotationJoint = new GameObject();
        GunRotationJoint.name = "GunRotationJoint";
        GunRotationJoint.transform.parent = CapRotationJoint.transform;
        GunRotationJoint.transform.localPosition = new Vector3(0, 0.25f, -0.30f);
        GunRotationJoint.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //HeadRing1
        HeadRing1 = new GameObject();
        HeadRing1.name = "HeadRing1";
        meshRenderer = HeadRing1.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = HeadRing1.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(ringProfile, MeshUtilities.MakeCirclePath(0.5f,detail), true);
        HeadRing1.transform.parent = Head.transform;
        HeadRing1.transform.localPosition = new Vector3(-0, 0.205f, 0);
        HeadRing1.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //HeadRing2
        HeadRing2 = new GameObject();
        HeadRing2.name = "HeadRing2";
        meshRenderer = HeadRing2.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = HeadRing2.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(ringProfile, MeshUtilities.MakeCirclePath(0.48f, detail), true);
        HeadRing2.transform.parent = Head.transform;
        HeadRing2.transform.localPosition = new Vector3(-0, 0.04f, 0);
        HeadRing2.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //HeadRing3
        HeadRing3 = new GameObject();
        HeadRing3.name = "HeadRing3";
        meshRenderer = HeadRing3.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = HeadRing3.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(ringProfile, MeshUtilities.MakeCirclePath(0.46f, detail), true);
        HeadRing3.transform.parent = Head.transform;
        HeadRing3.transform.localPosition = new Vector3(-0, -0.12f, 0);
        HeadRing3.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));


        //HeadColumns Path
        Matrix4x4[] HeadColumnsPath = new Matrix4x4[6];
        HeadColumnsPath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        HeadColumnsPath[1] = Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        HeadColumnsPath[2] = Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        HeadColumnsPath[3] = Matrix4x4.Scale(new Vector3(0.95f, 0.95f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.5f)); // Bottom
        HeadColumnsPath[4] = Matrix4x4.Scale(new Vector3(0.95f, 0.95f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.5f)); // Bottom
        HeadColumnsPath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.5f)); // Bottom

        //HeadColumns
        HeadColumns = new GameObject();
        HeadColumns.name = "HeadColumns";
        meshRenderer = HeadColumns.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = HeadColumns.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(JoshUtilities.MakeWavyCircleProfile(0.11f, 64, 0.38f, 8f), HeadColumnsPath, true);
        HeadColumns.transform.parent = Head.transform;
        HeadColumns.transform.localPosition = new Vector3(0, 0.25f, 0);
        HeadColumns.transform.localRotation = Quaternion.Euler(new Vector3(90, -10, 0));

        //CapBottom Path
        Matrix4x4[] CapBottomPath = new Matrix4x4[6];
        CapBottomPath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        CapBottomPath[1] = Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        CapBottomPath[2] = Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        CapBottomPath[3] = Matrix4x4.Scale(new Vector3(0.9f, 0.9f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.12f)); // Bottom
        CapBottomPath[4] = Matrix4x4.Scale(new Vector3(0.9f, 0.9f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.12f)); // Bottom
        CapBottomPath[5] = Matrix4x4.Scale(new Vector3(0, 0, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.12f)); // Bottom

        //CapBottom
        CapBottom = new GameObject();
        CapBottom.name = "CapBottom";
        meshRenderer = CapBottom.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = CapBottom.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(0.5f, detail), CapBottomPath, false);
        CapBottom.transform.parent = CapRotationJoint.transform;
        CapBottom.transform.localPosition = new Vector3(0, 0, 0);
        CapBottom.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

        //CapTop
        CapTop = new GameObject();
        CapTop.name = "CapTop";
        meshRenderer = CapTop.AddComponent<MeshRenderer>();
        if (BaseColor == null)
        {
            BaseColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BaseColor;
        meshFilter = CapTop.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(0.919f, detail), JoshUtilities.MakeSpherePath(detail, 1.21f, 0.7f), false);
        CapTop.transform.parent = CapRotationJoint.transform;
        CapTop.transform.localPosition = new Vector3(0, -0.236f, 0);
        CapTop.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

        //GunWheel
        GunWheel = new GameObject();
        GunWheel.name = "GunWheel";
        meshRenderer = GunWheel.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        meshFilter = GunWheel.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cylinder(detail, 0.08f, 0.038f);
        GunWheel.transform.parent = GunRotationJoint.transform;
        GunWheel.transform.localPosition = new Vector3(0, 0, 0);
        GunWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

        //GunBarrel
        GunBarrel = new GameObject();
        GunBarrel.name = "GunBarrel";
        meshRenderer = GunBarrel.AddComponent<MeshRenderer>();
        if (ChromeColor == null)
        {
            ChromeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = ChromeColor;
        meshFilter = GunBarrel.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cylinder(detail, 0.025f, 0.3f);
        GunBarrel.transform.parent = GunRotationJoint.transform;
        GunBarrel.transform.localPosition = new Vector3(0, 0.0f, -0.28f);
        GunBarrel.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

        //GunMuzzel Path
        Matrix4x4[] GunMuzzelPath = new Matrix4x4[20];
        GunMuzzelPath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        GunMuzzelPath[1] = Matrix4x4.Scale(new Vector3(0.6f, 0.6f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0)); // Bottom
        GunMuzzelPath[2] = Matrix4x4.Scale(new Vector3(0.6f, 0.6f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.015f)); // Bottom
        GunMuzzelPath[3] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.015f)); // Bottom
        GunMuzzelPath[4] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.03f)); // Bottom
        GunMuzzelPath[5] = Matrix4x4.Scale(new Vector3(0.9f, 0.9f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.03f)); // Bottom
        GunMuzzelPath[6] = Matrix4x4.Scale(new Vector3(0.9f, 0.9f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.045f)); // Bottom
        GunMuzzelPath[7] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.045f)); // Bottom
        GunMuzzelPath[8] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.06f)); // Bottom
        GunMuzzelPath[9] = Matrix4x4.Scale(new Vector3(1.0f, 1.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.06f)); // Bottom
        GunMuzzelPath[10] = Matrix4x4.Scale(new Vector3(1.0f, 1.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.075f)); // Bottom
        GunMuzzelPath[11] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.075f)); // Bottom
        GunMuzzelPath[12] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.09f)); // Bottom
        GunMuzzelPath[13] = Matrix4x4.Scale(new Vector3(0.9f, 0.9f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.09f)); // Bottom
        GunMuzzelPath[14] = Matrix4x4.Scale(new Vector3(0.9f, 0.9f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.105f)); // Bottom
        GunMuzzelPath[15] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.105f)); // Bottom
        GunMuzzelPath[16] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.12f)); // Bottom
        GunMuzzelPath[17] = Matrix4x4.Scale(new Vector3(0.6f, 0.6f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.12f)); // Bottom
        GunMuzzelPath[18] = Matrix4x4.Scale(new Vector3(0.6f, 0.6f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.135f)); // Bottom
        GunMuzzelPath[19] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1)) * Matrix4x4.Translate(new Vector3(0, 0, 0.135f)); // Bottom

        //GunMuzzel
        GunMuzzel = new GameObject();
        GunMuzzel.name = "GunMuzzel";
        meshRenderer = GunMuzzel.AddComponent<MeshRenderer>();
        if (BallColor == null)
        {
            BallColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = BallColor;
        meshFilter = GunMuzzel.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(MeshUtilities.MakeCircleProfile(0.06f, detail), GunMuzzelPath, false);
        GunMuzzel.transform.parent = GunRotationJoint.transform;
        GunMuzzel.transform.localPosition = new Vector3(0, 0.0f, -0.48f);
        GunMuzzel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GunMuzzel.transform.localScale = new Vector3(1.20f, 1.20f, 0.88f);

        //GunTip
        GunTip = new GameObject();
        GunTip.name = "GunTip";
        meshRenderer = GunTip.AddComponent<MeshRenderer>();
        if (HeadTubeColor == null)
        {
            HeadTubeColor = new Material(Shader.Find("Standard"));
        }
        meshRenderer.sharedMaterial = HeadTubeColor;
        meshFilter = GunTip.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Sweep(JoshUtilities.MakeOvalProfile(0.04f,0.06f, detail), MeshUtilities.MakeCirclePath(0.04f, detail), true);
        GunTip.transform.parent = GunRotationJoint.transform;
        GunTip.transform.localPosition = new Vector3(0, 0.0f, -0.6f);
        GunTip.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

        //GunEye
        GunEye = new GameObject();
        GunEye.name = "GunEye";
        meshRenderer = GunEye.AddComponent<MeshRenderer>(); meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        meshFilter = GunEye.AddComponent<MeshFilter>();
        meshFilter.mesh = MeshUtilities.Cylinder(detail, 0.025f, 0.01f);
        GunEye.transform.parent = GunRotationJoint.transform;
        GunEye.transform.localPosition = new Vector3(0, 0.0f, -0.647f);
        GunEye.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

    }
}
