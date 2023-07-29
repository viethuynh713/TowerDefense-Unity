using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Sound
{
   public SoundID Id;
   public AudioClip SourceAudioClip;
   public bool Loop ;
   public bool PlayOnWake;
   [Range(0,250)]
   public int Priority;

}

public enum SoundID
{
   Lobby,
   InGame,
}
