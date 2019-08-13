using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;
using DG.Tweening;
using UnityEngine.Serialization;

public class ConfigReader : MonoBehaviour
{
    
    #region Values
    
    private int _snakeCount = 0;
    private int _snakeSize  = 0;
    private int _fieldXSize = 0;
    private int _fieldYSize = 0;
    private int _fieldZSize = 0;
    private int _appleCount = 0;
    private float _appleDelay = 0f;
    
    #endregion
        
    private void Awake()
    {
        local.init("local");
        
        _snakeCount = local.general.snake.count;
        _snakeSize = local.general.snake.start_size;

        _fieldXSize = local.general.field.width;
        _fieldYSize = local.general.field.height;
        _fieldZSize = local.general.field.depth;
        
        _appleCount = local.general.apple.count;
        _appleDelay = local.general.apple.delay;
    }

}
