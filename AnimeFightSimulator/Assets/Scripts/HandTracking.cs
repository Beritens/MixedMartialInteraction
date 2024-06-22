// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Sample.HandTracking
{
  public class HandTracking : ImageSourceSolution<HandTrackingGraph>
  {


    [SerializeField] private DetectionListAnnotationController _palmDetectionsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromPalmDetectionsAnnotationController;
    [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromLandmarksAnnotationController;
    [SerializeField] private List<Rigidbody> physicalHands;

    public List<GameObject> dotPrefabs;
    public List<Transform> RightHand;
    public List<Transform> LeftHand;
    private List<List<Transform>> HandObjects = new List<List<Transform>>();
    private Vector3 currPos = new Vector3(0, 0, 0);
    public List<List<Vector3>> hands = new List<List<Vector3>>();
    public List<Vector3> middles = new List<Vector3>();
    public float testZScale = 4;
    public float handScale = 3;
    

    public List<Transform> leftBones;

    public List<Transform> rightBones;
    public bool bones = false;


    public void Awake()
    {
      if (bones)
      {
        HandObjects.Add(leftBones);
        HandObjects.Add(rightBones);
      }
      hands.Add(new List<Vector3>());
      hands.Add(new List<Vector3>());
      middles.Add(Vector3.zero);
      middles.Add(Vector3.zero);
      if (!bones)
      {
        HandObjects.Add(new List<Transform>());
        HandObjects.Add(new List<Transform>());
      }
      for (int i = 0; i < 2; i++)
      {
        for (int j = 0; j < 21; j++)
        {
          hands[i].Add(new Vector3(0, 0, 0));
          //continue;
          var obj = Instantiate(dotPrefabs[i]);
          obj.name = j.ToString();
          HandObjects[i].Add(obj.transform);
        }
      }
    }

    public float scaling = 5;
    public float dist = 2;
    public void Update()
    {

      


      if (!bones)
      {
        for (int i = 0; i < 2; i++)
        {
          for (int j = 0; j < hands[i].Count; j++)
          {
              HandObjects[i][j].position = hands[i][j];
           
          }
        }
        return;
      }
      for (int i = 0; i < HandObjects.Count; i++)
      {
        //HandObjects[i][0].position = hands[i][0];
        HandObjects[i][0].position = physicalHands[i].position - (middles[i] - hands[i][0]);
        HandObjects[i][0].up = Vector3.forward;
        Vector3 ktk = hands[i][17] - hands[i][13];
        Vector3 btp = hands[i][13] - hands[i][0];
        Vector3 rot = Vector3.Cross(ktk, btp) * (Mathf.Pow(-1, i));
        //float angle = Vector3.Angle(rot, new Vector3(0, 0, 1));
        Quaternion rotation = Quaternion.LookRotation(btp, rot);
        Quaternion xSpin = Quaternion.Euler(90, 0, 0);
        Quaternion ySpin = Quaternion.Euler(0, 180, 0);
        HandObjects[i][0].rotation = rotation * xSpin;
        //HandObjects[i][0].RotateAround(HandObjects[i][0].position, HandObjects[i][0].up, angle);
        for (int j = 0; j < 5; j++)
        {
          int m = 4;
          int pointOffset = 1;
          int finger = j * 4;
          if (j == 0)
          {
            //pointOffset = 0;
            finger += 1;
            m = 3;
          }
          int oldIndex = 0;
          for (int k = 0; k < m; k++)
          {
            int index = finger + k;

            Vector3 p1 = hands[i][index + pointOffset];

            Vector3 p2 = hands[i][oldIndex];
            Vector3 diff = p1 - p2;
            
            HandObjects[i][index].up = diff;
            float curRot = HandObjects[i][index].localEulerAngles.y;
            Quaternion corRot = Quaternion.Euler(0,k==0? 180-curRot:-curRot,0);
            HandObjects[i][index].localRotation *= corRot;
            oldIndex = index + pointOffset;

          }

        }
      }

    }
    public HandTrackingGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
      get => graphRunner.maxNumHands;
      set => graphRunner.maxNumHands = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    protected override void OnStartRun()
    {

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_palmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromPalmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromLandmarksAnnotationController, imageSource, true);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    Vector3 screenToWorld(Vector3 a)
    {
      return Vector3.Scale(a, new Vector3(16f / 9f, -1, -16f/9f));
    }
    protected override IEnumerator WaitForNextValue()
    {
      var task = graphRunner.WaitNext();
      yield return new WaitUntil(() => task.IsCompleted);

      var result = task.Result;

      var handednussy = result.handedness;

      // if (handednussy != null)
      // {
      //   if (handednussy.Count > 0)
      //   {
      //     //print(value[0].Classification);
      //   }
      // }

      var landmarks = result.handWorldLandmarks;
      var screenMarks = result.handLandmarks;
      middles[0] = Vector3.zero;
      middles[1] = Vector3.zero;
      if (landmarks != null)
      {
        for (int h = 0; h < landmarks.Count && h < 2; h++)
        {
          NormalizedLandmark baseLandmark = screenMarks[h].Landmark[0];
          NormalizedLandmark pinky = screenMarks[h].Landmark[17];
          Vector3 screenScale = new Vector3(pinky.X - baseLandmark.X, pinky.Y - baseLandmark.Y, pinky.Z - baseLandmark.Z);
          screenScale = screenToWorld(screenScale);
          
          Landmark baseHand = landmarks[h].Landmark[0];
          Landmark pinkyHand = landmarks[h].Landmark[17];
          Vector3 baseVector = new Vector3(baseHand.X, baseHand.Y, baseHand.Z);
          Vector3 worldScale = new Vector3(pinkyHand.X - baseHand.X, pinkyHand.Y - baseHand.Y, pinkyHand.Z - baseHand.Z);

          float ratio = worldScale.magnitude / screenScale.magnitude;
          Vector3 base_pos = new Vector3(ratio*(baseLandmark.X-0.5f) * (16f/9f),ratio*(-baseLandmark.Y + 0.5f), dist-ratio);
          for (int i = landmarks[h].Landmark.Count - 1; i >= 0; i--)
          {

            Mediapipe.Landmark landmark = landmarks[h].Landmark[i];
            // hands[h][i].x = landmark.X;
            // hands[h][i].y = landmark.Y;
            // hands[h][i].z = landmark.Z * 100;
            int index = handednussy[h].Classification[0].Label == "Left" ? 0 : 1;
            hands[index][i] = base_pos*handScale + (Vector3.Scale(new Vector3(landmark.X, landmark.Y, landmark.Z)-baseVector, new Vector3(handScale,-handScale,-handScale)));
            middles[index] += hands[index][i] * (1f / 21f);

          }
          //currPos = new Vector3(landmark.X, landmark.Y, landmark.Z);
        }

      }




      //_palmDetectionsAnnotationController.DrawNow(result.palmDetections);
      //_handRectsFromPalmDetectionsAnnotationController.DrawNow(result.handRectsFromPalmDetections);
      //_handLandmarksAnnotationController.DrawNow(result.handLandmarks, result.handedness);
      // TODO: render HandWorldLandmarks annotations
      //_handRectsFromLandmarksAnnotationController.DrawNow(result.handRectsFromLandmarks);
    }


  }
}
