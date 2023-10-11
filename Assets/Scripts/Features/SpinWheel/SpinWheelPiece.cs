using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SpinWheel
{
    public sealed class SpinWheelPiece : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        private float _winningAngleMin;
        private float _winningAngleMax;
        
        public int Index { get; private set; }
        public int Value { get; private set; }
        
        public void Setup(int index, float pieceAngle, Color pieceColor, int value)
        {
            Index = index;
            Value = value;
            
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillOrigin = (int)Image.Origin360.Top;
            image.fillAmount = pieceAngle / 360f;

            image.color = pieceColor;

            text.text = value.ToString();
            
            transform.localEulerAngles = new Vector3(0, 0, -index * pieceAngle);

            _winningAngleMin = index * pieceAngle;
            _winningAngleMax = _winningAngleMin + pieceAngle;
        }

        public bool IsWinning(float wheelAngle)
        {
            return wheelAngle >= _winningAngleMin && wheelAngle < _winningAngleMax;
        }
    }
}