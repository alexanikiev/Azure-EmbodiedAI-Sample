using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;
using UnityEngine.UI;

namespace AzureEmbodiedAISamples
{
    public class TrackerHandler : MonoBehaviour
    {
        public Dictionary<JointId, JointId> parentJointMap;
        Dictionary<JointId, Quaternion> basisJointMap;
        //public Text stateIndicator;

        // Start is called before the first frame update
        void Awake()
        {
            parentJointMap = new Dictionary<JointId, JointId>();

            // pelvis has no parent so set to count
            parentJointMap[JointId.Pelvis] = JointId.Count;
            parentJointMap[JointId.SpineNavel] = JointId.Pelvis;
            parentJointMap[JointId.SpineChest] = JointId.SpineNavel;
            parentJointMap[JointId.Neck] = JointId.SpineChest;
            parentJointMap[JointId.ClavicleLeft] = JointId.SpineChest;
            parentJointMap[JointId.ShoulderLeft] = JointId.ClavicleLeft;
            parentJointMap[JointId.ElbowLeft] = JointId.ShoulderLeft;
            parentJointMap[JointId.WristLeft] = JointId.ElbowLeft;
            parentJointMap[JointId.HandLeft] = JointId.WristLeft;
            parentJointMap[JointId.HandTipLeft] = JointId.HandLeft;
            parentJointMap[JointId.ThumbLeft] = JointId.HandLeft;
            parentJointMap[JointId.ClavicleRight] = JointId.SpineChest;
            parentJointMap[JointId.ShoulderRight] = JointId.ClavicleRight;
            parentJointMap[JointId.ElbowRight] = JointId.ShoulderRight;
            parentJointMap[JointId.WristRight] = JointId.ElbowRight;
            parentJointMap[JointId.HandRight] = JointId.WristRight;
            parentJointMap[JointId.HandTipRight] = JointId.HandRight;
            parentJointMap[JointId.ThumbRight] = JointId.HandRight;
            parentJointMap[JointId.HipLeft] = JointId.SpineNavel;
            parentJointMap[JointId.KneeLeft] = JointId.HipLeft;
            parentJointMap[JointId.AnkleLeft] = JointId.KneeLeft;
            parentJointMap[JointId.FootLeft] = JointId.AnkleLeft;
            parentJointMap[JointId.HipRight] = JointId.SpineNavel;
            parentJointMap[JointId.KneeRight] = JointId.HipRight;
            parentJointMap[JointId.AnkleRight] = JointId.KneeRight;
            parentJointMap[JointId.FootRight] = JointId.AnkleRight;
            parentJointMap[JointId.Head] = JointId.Pelvis;
            parentJointMap[JointId.Nose] = JointId.Head;
            parentJointMap[JointId.EyeLeft] = JointId.Head;
            parentJointMap[JointId.EarLeft] = JointId.Head;
            parentJointMap[JointId.EyeRight] = JointId.Head;
            parentJointMap[JointId.EarRight] = JointId.Head;

            Vector3 zpositive = Vector3.forward;
            Vector3 xpositive = Vector3.right;
            Vector3 ypositive = Vector3.up;
            // spine and left hip are the same
            Quaternion leftHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
            Quaternion spineHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
            Quaternion rightHipBasis = Quaternion.LookRotation(xpositive, zpositive);
            // arms and thumbs share the same basis
            Quaternion leftArmBasis = Quaternion.LookRotation(ypositive, -zpositive);
            Quaternion rightArmBasis = Quaternion.LookRotation(-ypositive, zpositive);
            Quaternion leftHandBasis = Quaternion.LookRotation(-zpositive, -ypositive);
            Quaternion rightHandBasis = Quaternion.identity;
            Quaternion leftFootBasis = Quaternion.LookRotation(xpositive, ypositive);
            Quaternion rightFootBasis = Quaternion.LookRotation(xpositive, -ypositive);

            basisJointMap = new Dictionary<JointId, Quaternion>();

            // pelvis has no parent so set to count
            basisJointMap[JointId.Pelvis] = spineHipBasis;
            basisJointMap[JointId.SpineNavel] = spineHipBasis;
            basisJointMap[JointId.SpineChest] = spineHipBasis;
            basisJointMap[JointId.Neck] = spineHipBasis;
            basisJointMap[JointId.ClavicleLeft] = leftArmBasis;
            basisJointMap[JointId.ShoulderLeft] = leftArmBasis;
            basisJointMap[JointId.ElbowLeft] = leftArmBasis;
            basisJointMap[JointId.WristLeft] = leftHandBasis;
            basisJointMap[JointId.HandLeft] = leftHandBasis;
            basisJointMap[JointId.HandTipLeft] = leftHandBasis;
            basisJointMap[JointId.ThumbLeft] = leftArmBasis;
            basisJointMap[JointId.ClavicleRight] = rightArmBasis;
            basisJointMap[JointId.ShoulderRight] = rightArmBasis;
            basisJointMap[JointId.ElbowRight] = rightArmBasis;
            basisJointMap[JointId.WristRight] = rightHandBasis;
            basisJointMap[JointId.HandRight] = rightHandBasis;
            basisJointMap[JointId.HandTipRight] = rightHandBasis;
            basisJointMap[JointId.ThumbRight] = rightArmBasis;
            basisJointMap[JointId.HipLeft] = leftHipBasis;
            basisJointMap[JointId.KneeLeft] = leftHipBasis;
            basisJointMap[JointId.AnkleLeft] = leftHipBasis;
            basisJointMap[JointId.FootLeft] = leftFootBasis;
            basisJointMap[JointId.HipRight] = rightHipBasis;
            basisJointMap[JointId.KneeRight] = rightHipBasis;
            basisJointMap[JointId.AnkleRight] = rightHipBasis;
            basisJointMap[JointId.FootRight] = rightFootBasis;
            basisJointMap[JointId.Head] = spineHipBasis;
            basisJointMap[JointId.Nose] = spineHipBasis;
            basisJointMap[JointId.EyeLeft] = spineHipBasis;
            basisJointMap[JointId.EarLeft] = spineHipBasis;
            basisJointMap[JointId.EyeRight] = spineHipBasis;
            basisJointMap[JointId.EarRight] = spineHipBasis;
        }

        public void updateTracker(BackgroundData trackerFrameData)
        {
            //this is an array in case you want to get the n closest bodies
            int closestBody = findClosestTrackedBody(trackerFrameData);

            // render the closest body
            Body skeleton = trackerFrameData.Bodies[closestBody];
            // TODO:
        }

        private int findClosestTrackedBody(BackgroundData trackerFrameData)
        {
            int closestBody = -1;
            const float MAX_DISTANCE = 5000.0f;
            float minDistanceFromKinect = MAX_DISTANCE;
            for (int i = 0; i < (int)trackerFrameData.NumOfBodies; i++)
            {
                var pelvisPosition = trackerFrameData.Bodies[i].JointPositions3D[(int)JointId.Pelvis];
                Vector3 pelvisPos = new Vector3((float)pelvisPosition.X, (float)pelvisPosition.Y, (float)pelvisPosition.Z);
                if (pelvisPos.magnitude < minDistanceFromKinect)
                {
                    closestBody = i;
                    minDistanceFromKinect = pelvisPos.magnitude;
                }
            }
            return closestBody;
        }
    }
}