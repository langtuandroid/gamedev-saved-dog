using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    // Anim Doge
    public const string DOGE_ANIM_IDLE = "start_3";
    public const string DOGE_ANIM_SCARE = "scare";
    public const string DOGE_ANIM_GET_HIT = "get_hit";
    public const string DOGE_ANIM_HURT = "lose";
    public const string DOGE_ANIM_DIE = "die";
    public const string DOGE_ANIM_WIN = "win";

    // Skin Doge
    public const int DOGE_SKIN_SHIBA = 7;
    public const int DOGE_SKIN_HUSKY = 8;
    public const int DOGE_SKIN_BULK = 9;
    public const int DOGE_SKIN_CAPTAIN = 11;
    public const int DOGE_SKIN_IRONMAN = 12;
    public const int DOGE_SKIN_ELF = 13;
    public const int DOGE_SKIN_VALKYRIE = 14;
    public const int DOGE_SKIN_FLASH = 15;
    public const int DOGE_SKIN_TREE = 16;
    public const int DOGE_SKIN_MINION = 17;
    public const int DOGE_SKIN_RUGBY = 18;
    public const int DOGE_SKIN_SANTA = 19;
    public const int DOGE_SKIN_BATMAN = 20;
    public int[] skins = {10, 1, 29, 3, 2, 11, 12 , 13, 17,20,22,23,21,18,19,15, 16,24};

    // Audio
    public const string AUDIO_MUSIC_BG = "Background menu";
    public const string AUDIO_MUSIC_SHOP = "Shop Music";

    public const string AUDIO_SFX_BUTTON = "Button";
    public const string AUDIO_SFX_PLAY = "Play";
    public const string AUDIO_SFX_BEE = "Bee sound";
    public const string AUDIO_SFX_STING = "Sting";
    public const string AUDIO_SFX_DOGHURT = "Hurt";
    public const string AUDIO_SFX_LOSE = "Lose";
    public const string AUDIO_SFX_LOSE_UI = "LoseUI";
    public const string AUDIO_SFX_WOOHOO = "Wooh";
    public const string AUDIO_SFX_WIN = "Win";
    public const string AUDIO_SFX_BTNSETTINGS = "BtnInSetting";
    public const string AUDIO_SFX_COIN = "CoinGain";
    public const string AUDIO_SFX_BEE_STAB = "Bee Stab";
    public const string AUDIO_SFX_BEE_DEAD = "Bee Dead";
    public const string AUDIO_SFX_BLADE = "Blade";
    public const string AUDIO_SFX_HEADSHOT = "Headshot";
    public const string AUDIO_SFX_COIN_DROP = "Coin Drop";
    public const string AUDIO_SFX_GOOD = "Good";
    public const string AUDIO_SFX_GREAT = "Great";
    public const string AUDIO_SFX_EXCELLENT = "Excellent";
    public const string AUDIO_SFX_AMAZING = "Amazing";
    public const string AUDIO_SFX_AWESOME = "Awesome";
    public const string AUDIO_SFX_INCREDIBLE = "Incredible";
    public const string AUDIO_SFX_UNBELIEVABLE = "Unbelievable";

    // Object Entity
    public const string DOGE = "Doge";
    public const string BEE = "Bee";
    public const string DMG_TEXT = "TextDmg";
    public const string SAW = "Saw";

    // Particle
    public const string PAR_BLOOD_VFX = "BloodEffect";
    public const string PAR_KNOCK_VFX = "Knock";
    public const string HEADSHOT_VFX = "Headshot";
    public const string KILL_VFX = "Kill";
    public const string KILL_COIN_VFX = "Kill Coin";
}
