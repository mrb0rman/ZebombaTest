using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using ZebombaTest.Scripts.UI;
using Zenject;

namespace ZebombaTest.Scripts
{
    namespace GameSystem
    {
        public class GameController : ITickable
        {
            private readonly BallView.Pool _ballPool;
            private readonly CameraView _cameraView;
            private readonly TickableManager _tickableManager;
            private readonly IUIService _uiService;
            private readonly ParticleEffectView.Pool _particlePool;
            
            private BallView[,] _gridBallView = new BallView[3, 3];
            
            private float[] _borders =
            {
                0.33f,
                0.66f
            };

            private float[] _columnCenter =
            {
                0.165f,
                0.5f,
                0.825f
            };
            
            private Vector2 _leftVP;
            private Vector2 _apexVP;
            private Vector2 _rightVP;
            private Vector2 _center;
            
            private BallView _currentBallView;
            private Tween _currentTween;
            private bool _isReady;
            private int _currentCountBallView;
            private int _currentScore;
            
            public GameController(
                BallView.Pool ballPool,
                CameraView cameraView,
                TickableManager tickableManager,
                IUIService uiService,
                ParticleEffectView.Pool particlePool)
            {
                _ballPool = ballPool;
                _cameraView = cameraView;
                _tickableManager = tickableManager;
                _uiService = uiService;
                _particlePool = particlePool;

                _leftVP  = _cameraView.Camera.ViewportToWorldPoint(new Vector3(0.165f,  0.8f,  _cameraView.Camera.nearClipPlane));
                _apexVP  = _cameraView.Camera.ViewportToWorldPoint(new Vector3(0.5f,  0.7f,  _cameraView.Camera.nearClipPlane));
                _rightVP = _cameraView.Camera.ViewportToWorldPoint(new Vector3(0.825f, 0.8f, _cameraView.Camera.nearClipPlane));
                _center = _cameraView.Camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, _cameraView.Camera.nearClipPlane));
            }

            public void Init()
            {
                _currentScore = 0;
                
                DOVirtual.DelayedCall(0.75f, () =>
                {
                    CreateBall();
                    _tickableManager.Add(this);
                    _isReady = true;
                });
            }
            
            public void Tick()
            {
                if (Input.GetMouseButtonDown(0) && _isReady)
                {
                    CheckBallGrid();
                }
            }
            
            private void CreateBall()
            {
                _currentTween.Kill();
                _currentTween = null;
                
                _currentBallView = _ballPool.Spawn();
                
                StartAnimationMoveBall();
                _isReady = true;
            }
            
            private void StartAnimationMoveBall()
            {
                Vector3[] path = { _leftVP, _apexVP, _rightVP };
                
                _currentBallView.transform.position = path[0];
                
                _currentTween = _currentBallView.transform.DOPath(path, 1f, PathType.CatmullRom, PathMode.TopDown2D)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .OnUpdate(() =>
                    {
                        _currentBallView.LineRenderer.SetPosition(0, _currentBallView.transform.position);
                        _currentBallView.LineRenderer.SetPosition(1, _center);
                    })
                    .OnKill(() =>
                    {
                        _currentBallView.LineRenderer.SetPosition(0, _currentBallView.transform.position);
                        _currentBallView.LineRenderer.SetPosition(1, _currentBallView.transform.position);
                    });
            }

            private void CheckBallGrid()
            {
                _isReady = false;
                
                var positionBallView = _currentBallView.transform.position;
                var worldPos = _cameraView.Camera.WorldToViewportPoint(new Vector3(positionBallView.x, positionBallView.y, positionBallView.z));
                var currentColumn = 1;
                
                if (worldPos.x < _borders[0])
                {
                    currentColumn = 0;
                }
                else if (worldPos.x > _borders[1])
                {
                    currentColumn = 2;
                }
                
                for (var i = 0; i < _gridBallView.GetLength(1); i++)
                {
                    if (_gridBallView[currentColumn, i] != null) continue;
                    
                    _currentTween.Kill();
                    _currentTween = null;
                    
                    _gridBallView[currentColumn, i] = _currentBallView;
                    AddBallInGrid(currentColumn, i);
                    return;
                }
                
                _isReady = true;
            }

            private void AddBallInGrid(int i, int j)
            {
                _currentCountBallView++;
                
                _currentBallView.transform.DOMove(CellToWorld(i, j, _currentBallView.SpriteRenderer.bounds.size.y), 0.5f).SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                    {
                        _currentBallView = null;
                        
                        if (TryFindMatches(out var cells))
                        {
                            ClearAndCollapse(cells);
                        }
                        
                        if (_currentCountBallView == 9)
                        {
                            EndGame();
                            return;
                        }
                        
                        CreateBall();
                        _isReady = true;
                    });
            }

            
            private bool TryFindMatches(out List<(int c, int r)> cells)
            {
                cells = new List<(int, int)>();

                for (var r = 0; r < 3; r++)
                {
                    var a = _gridBallView[0,r]; 
                    var b = _gridBallView[1,r]; 
                    var c0 = _gridBallView[2,r];
                    if (Valid(a) && Valid(b) && Valid(c0) && 
                        a.SpriteRenderer.color == b.SpriteRenderer.color && b.SpriteRenderer.color == c0.SpriteRenderer.color)
                        cells.AddRange(new[]{ (0,r), (1,r), (2,r) });
                }
                
                for (var c = 0; c < 3; c++)
                {
                    var a = _gridBallView[c,0]; 
                    var b = _gridBallView[c,1]; 
                    var d = _gridBallView[c,2];
                    if (Valid(a) && Valid(b) && Valid(d) && 
                        a.SpriteRenderer.color == b.SpriteRenderer.color && b.SpriteRenderer.color == d.SpriteRenderer.color)
                        cells.AddRange(new[]{ (c,0), (c,1), (c,2) });
                }
                
                var d1 = new[]{ _gridBallView[0,0], _gridBallView[1,1], _gridBallView[2,2] };
                if (Valid(d1[0]) && Valid(d1[1]) && Valid(d1[2]) 
                    && d1[0].SpriteRenderer.color==d1[1].SpriteRenderer.color && d1[1].SpriteRenderer.color==d1[2].SpriteRenderer.color)
                    cells.AddRange(new[]{ (0,0), (1,1), (2,2) });

                var d2 = new[]{ _gridBallView[0,2], _gridBallView[1,1], _gridBallView[2,0] };
                if (Valid(d2[0]) && Valid(d2[1]) && Valid(d2[2]) && 
                    d2[0].SpriteRenderer.color==d2[1].SpriteRenderer.color && d2[1].SpriteRenderer.color==d2[2].SpriteRenderer.color)
                    cells.AddRange(new[]{ (0,2), (1,1), (2,0) });
                
                if (cells.Count == 0) return false;
                cells = new List<(int,int)>(new HashSet<(int,int)>(cells));
                return true;

                bool Valid(BallView v) => v != null;
            }

            private void ClearAndCollapse(List<(int c,int r)> cellsToClear)
            {
                foreach (var (c,r) in cellsToClear)
                {
                    var ballView = _gridBallView[c,r];
                    _gridBallView[c,r] = null;

                    _currentScore += ballView.Cost;

                    var particleEffectView = _particlePool.Spawn();
                    var mainParticle = particleEffectView.ParticleSystem.main;
                    mainParticle.startColor = ballView.SpriteRenderer.color;
                    
                    particleEffectView.transform.position = ballView.transform.position;
                    particleEffectView.ParticleSystem.Play();
                    DOVirtual.DelayedCall(particleEffectView.ParticleSystem.main.duration, () =>
                    {
                        _particlePool.Despawn(particleEffectView);
                    });
                    
                    _ballPool.Despawn(ballView);
                    _currentCountBallView--;
                }
                
                for (var c = 0; c < 3; c++)
                {
                    var writeRow = 0;
                    for (var readRow = 0; readRow < 3; readRow++)
                    {
                        var v = _gridBallView[c, readRow];
                        if (v == null) continue;

                        if (readRow != writeRow)
                        {
                            _gridBallView[c, readRow] = null;
                            _gridBallView[c, writeRow] = v;

                            var target = CellToWorld(c, writeRow, v.SpriteRenderer.bounds.size.y);
                            v.transform.DOKill();
                            v.transform.DOMove(target, 0.2f).SetEase(Ease.InQuad);
                        }

                        writeRow++;
                    }
                }
            }
            
            private void EndGame()
            {
                _tickableManager.Remove(this);
                
                _uiService.Hide<UIGameWindow>();
                
                var uiEndGameWindow = _uiService.Show<UIEndGameWindow>();
                uiEndGameWindow.SetScore(_currentScore);
                
                for (var i = 0; i < _gridBallView.GetLength(0); i++)
                {
                    for (var j = 0; j < _gridBallView.GetLength(1); j++)
                    {
                        _ballPool.Despawn(_gridBallView[i,j]);
                        _gridBallView[i, j] = null;
                    }
                }
               
                _currentCountBallView = 0;
                
                _currentTween.Kill();
                _currentTween = null;
                _currentBallView = null;
            }

            private Vector3 CellToWorld(int i, int j, float height)
            {
                var worldPos = _cameraView.Camera.ViewportToWorldPoint(new Vector3(_columnCenter[i], 0f, _cameraView.Camera.nearClipPlane));
                worldPos.y += height/2f + j * height;

                return worldPos;
            }
        }
    }
}