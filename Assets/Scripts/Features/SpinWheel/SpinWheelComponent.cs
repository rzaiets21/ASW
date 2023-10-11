using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.SpinWheel
{
    public sealed class SpinWheelComponent : MonoBehaviour
    {
        [SerializeField, Range(5, 20), Header("Settings")] private int piecesCount;

        [SerializeField] private float spinSpeed;
        [SerializeField] private float spinDuration;
        [SerializeField] private float randomSpinDurationOffset;
        [SerializeField] private float spinStoppingTime;

        [SerializeField] private AudioSource tickAudioSource;
        
        [SerializeField] private Gradient wheelGradientPalette;

        [SerializeField] private RectTransform piecesHolder;
        [SerializeField] private RectTransform linesHolder;
        [SerializeField] private SpinWheelPiece spinWheelPiecePrefab;
        [SerializeField] private Line linePrefab;

        private List<SpinWheelPiece> _pieces;

        private bool _isSpinStopping;
        private float _spinDurationTime;
        
        private float _time;

        private float _lastPieceAngleTick;
        private float _pieceAngle;

        public event Action OnSpinStarted;
        public event Action<int> OnSpinStopped;
        
        public bool IsSpinning { get; private set; }

        public void Init()
        {
            _pieces ??= new List<SpinWheelPiece>(piecesCount);
            
            var pieces = piecesHolder.GetComponentsInChildren<SpinWheelPiece>();
            var lines = linesHolder.GetComponentsInChildren<Line>();
            
            _pieceAngle = 360f / piecesCount;
            var piecesValues = GenerateRandomValues(10, 1000);
            
            for (int i = 0; i < piecesCount; i++)
            {
                var piece = pieces.Length > i ? pieces[i] : Instantiate(spinWheelPiecePrefab, piecesHolder);
                var line = lines.Length > i ? lines[i] : Instantiate(linePrefab, linesHolder);
                var pieceColor = wheelGradientPalette.Evaluate((float)i/piecesCount);
                
                line.transform.localEulerAngles = new Vector3(0, 0, i * _pieceAngle);
                piece.Setup(i, _pieceAngle, pieceColor, piecesValues[i]);
                
                piece.gameObject.SetActive(true);
                line.gameObject.SetActive(true);
                
                _pieces.Add(piece);
            }

            for (int i = piecesCount; i < pieces.Length; i++)
            {
                var piece = pieces[i];
                piece.gameObject.SetActive(false);
            }

            for (int i = piecesCount; i < lines.Length; i++)
            {
                var line = lines[i];
                line.gameObject.SetActive(false);
            }

            SetRandomRotation();
        }

        public void Spin()
        {
            if(IsSpinning)
                return;

            StartSpin();
        }

        private void Update()
        {
            if(!IsSpinning)
                return;

            var timeMultiplier = _isSpinStopping ? -1 : 1;
            _time += Time.deltaTime * timeMultiplier;

            var speed = (_time / spinStoppingTime) * spinSpeed;
            speed = Mathf.Clamp(speed, 0, spinSpeed);
            
            transform.Rotate(Vector3.forward * (speed * -1));

            TryPlayTickSound();
            
            if (_isSpinStopping && _time <= 0)
            {
                Stopped();
                return;
            }
            
            if(IsSpinning && _time >= _spinDurationTime)
                StopSpin();
        }

        private void TryPlayTickSound()
        {
            var currentRotation = transform.localEulerAngles.z;
            if (Mathf.Abs(_lastPieceAngleTick - currentRotation) > _pieceAngle)
            {
                PlayTickSound();
                _lastPieceAngleTick = currentRotation;
            }
                
        }

        private void PlayTickSound()
        {
            tickAudioSource.Play();
        }

        private void Stopped()
        {
            ResetWheelState();
            OnSpinStopped?.Invoke(GetWinningPiece().Value);
        }
        
        private void ResetWheelState()
        {
            _time = 0;
            IsSpinning = false;
            _isSpinStopping = false;
        }
        
        private void StartSpin()
        {
            _time = 0;
            IsSpinning = true;
            
            _spinDurationTime = spinDuration + Random.Range(0, randomSpinDurationOffset);
            
            OnSpinStarted?.Invoke();
        }

        private void StopSpin()
        {
            _time = spinStoppingTime;
            _isSpinStopping = true;
        }

        private SpinWheelPiece GetWinningPiece()
        {
            return _pieces.FirstOrDefault(x => x.IsWinning(transform.localEulerAngles.z));
        }
        
        private void SetRandomRotation()
        {
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360f));

            _lastPieceAngleTick = transform.localEulerAngles.z;
        }
        
        private int[] GenerateRandomValues(int min, int max)
        {
            var values = new int[piecesCount];
            
            for (int i = 0; i < piecesCount; i++)
            {
                var generatedValue = Random.Range(min, max);
                
                if(i == 0)
                {
                    values[i] = generatedValue;
                    continue;
                }

                var prevIndex = i - 1;
                if (prevIndex < 0)
                    prevIndex = piecesCount - 1;

                var nextIndex = i + 1;
                if (nextIndex >= piecesCount)
                    nextIndex = 0;
                
                while (Mathf.Abs(generatedValue - values[prevIndex]) < 10 || Mathf.Abs(generatedValue - values[nextIndex]) < 10 || values.Contains(generatedValue))
                {
                    generatedValue = Random.Range(min, max);
                }
                
                values[i] = generatedValue;
            }

            for (int i = 0; i < piecesCount; i++)
            {
                values[i] *= 100;
            }

            return values;
        }
    }
}