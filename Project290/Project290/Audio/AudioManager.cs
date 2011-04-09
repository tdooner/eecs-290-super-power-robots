//////__     __               _                 _     _               _   
//////\ \   / /              | |               | |   | |             | |  
////// \ \_/ /__  _   _   ___| |__   ___  _   _| | __| |  _ __   ___ | |_ 
//////  \   / _ \| | | | / __| '_ \ / _ \| | | | |/ _` | | '_ \ / _ \| __|
//////   | | (_) | |_| | \__ \ | | | (_) | |_| | | (_| | | | | | (_) | |_ 
//////   |_|\___/ \__,_| |___/_| |_|\___/ \__,_|_|\__,_| |_| |_|\___/ \__|
//////      _                              _   _     _     _ 
//////     | |                            | | | |   (_)   | |
//////  ___| |__   __ _ _ __   __ _  ___  | |_| |__  _ ___| |
////// / __| '_ \ / _` | '_ \ / _` |/ _ \ | __| '_ \| / __| |
//////| (__| | | | (_| | | | | (_| |  __/ | |_| | | | \__ \_|
////// \___|_| |_|\__,_|_| |_|\__, |\___|  \__|_| |_|_|___(_)
//////                         __/ |                         
//////                        |___/                          

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Project290.GameElements;

namespace Project290.Audio
{
    /// <summary>
    /// Manages playing of sounds and music.
    /// </summary>
    public class AudioManager : GameComponent
    {
        #region Private fields

        private Dictionary<string, Song> _songs = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();

        private Song _currentSong = null;
        private SoundEffectInstance[] _playingSounds = new SoundEffectInstance[MaxSounds];

        private bool _isMusicPaused = false;

        private bool _isFading = false;
        public float PauseMusicVolumeBackup;
        private MusicFadeEffect _fadeEffect;
        private Random rando = new Random();
        #endregion

        // Max number of sounds that can be playing at one time
        private const int MaxSounds = 32;

        /// <summary>
        /// Gets name of currently playing song, or null if no song is playing.
        /// </summary>
        public string CurrentSong { get; private set; }

        /// <summary>
        /// Gets and sets the volume of the music
        /// 1.0f is max volume
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = MathHelper.Clamp(value, 0f, 1f); }
        }

        /// <summary>
        /// Gets and sets the "master" volume for all sounds
        /// 1.0f is max volume
        /// </summary>
        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = MathHelper.Clamp(value, 0f, 1f); }
        }

        /// <summary>
        /// Is song still playing or paused?
        /// </summary>
        public bool IsSongActive
        {
            get { return _currentSong != null && MediaPlayer.State != MediaState.Stopped; }
        }

        /// <summary>
        /// Is song paused?
        /// </summary>
        public bool IsSongPaused
        {
            get { return _currentSong != null && _isMusicPaused; }
        }

        /// <summary>
        /// Creates a new Audio Manager. 
        /// </summary>
        /// <param name="game">The Game</param>
        public AudioManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Load a Song
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        public void LoadSong(string songName)
        {
            LoadSong(songName, songName);
        }

        /// <summary>
        /// Loads a Song
        /// </summary>
        /// <param name="songName">Name of the song</param>
        /// <param name="songPath">Path to the song</param> 
        public void LoadSong(string songName, string songPath)
        {
            if (_songs.ContainsKey(songName))
            {
                throw new InvalidOperationException(string.Format("Song '{0}' has already been loaded", songName));
            }
            try
            {
                _songs.Add(songName, GameWorld.content.Load<Song>(songPath));
            }
            catch { }
        }

        /// <summary>
        /// Loads a SoundEffect
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        /// <param name="soundPath">Path to the song asset file</param>
        public void LoadSound(string soundName, string soundPath)
        {
            if (_sounds.ContainsKey(soundName))
            {
                throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName)); 
            }

            try
            {
                _sounds.Add(soundName, GameWorld.content.Load<SoundEffect>(soundPath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void SongPlay(string songName)
        {
            SongPlay(songName, true);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void SongPlay(string songName, bool loop)
        {
            if (CurrentSong != songName && songName != null)
            {
                if (_currentSong != null)
                {
                    MediaPlayer.Stop();
                }

                if (!_songs.TryGetValue(songName, out _currentSong))
                {
                    throw new ArgumentException(string.Format("Song '{0}' not found", songName)); 
                }

                CurrentSong = songName;

                _isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(_currentSong);

                if (!Enabled)
                {
                    MediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public void PauseSong()
        {
            if (_currentSong != null && !_isMusicPaused)
            {
                if (Enabled) MediaPlayer.Pause();
                _isMusicPaused = true;
            }
        }

        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public void ResumeSong()
        {
            if (_currentSong != null && _isMusicPaused)
            {
                if (Enabled) MediaPlayer.Resume();
                _isMusicPaused = false;
            }
        }

        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public void StopSong()
        {
            if (_currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                _isMusicPaused = false;
            }
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void FadeSong(float targetVolume, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be a positive value"); 
            }

            _fadeEffect = new MusicFadeEffect(MediaPlayer.Volume, targetVolume, duration);
            _isFading = true;
        }
    
        /// <summary>
        /// Plays the sound of the given name
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public void PlaySound(string soundType)
        {
            PlaySound(soundType, 1.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Plays the sound of the given name at the given volume
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public void PlaySound(string soundType, float volume)
        {
            PlaySound(soundType, volume, 0.0f, 0.0f);
        }
        
        /// <summary>
        /// Plays the sound of the given name with parameters
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public void PlaySound(string soundName, float volume, float pitch, float pan)
        {
            SoundEffect sound;

            try
            {
                if (!_sounds.TryGetValue(soundName, out sound))
                {
                    throw new ArgumentException(string.Format("Sound '{0}' not found", soundName)); 
                }

                int index = GetAvailableSoundIndex();

                if (index != -1)
                {
                    _playingSounds[index] = sound.CreateInstance();
                    _playingSounds[index].Volume = volume;
                    _playingSounds[index].Pitch = pitch;
                    _playingSounds[index].Pan = pan;
                    _playingSounds[index].Play();

                    if (!Enabled)
                    {
                        _playingSounds[index].Pause();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Stops all currently playing sounds.
        /// </summary>
        public void StopAllSounds()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null)
                {
                    _playingSounds[i].Stop();
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }
        }

        /// <summary>
        /// Called per loop unless Enabled is set to false.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Stopped)
                {
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }

            if (_currentSong != null && MediaPlayer.State == MediaState.Stopped)
            {
                _currentSong = null;
                CurrentSong = null;
                _isMusicPaused = false;
            }

            if (_isFading && !_isMusicPaused)
            {
                if (_currentSong != null && MediaPlayer.State == MediaState.Playing)
                {
                    if (_fadeEffect.Update(gameTime.ElapsedGameTime))
                    {
                        _isFading = false;
                    }

                    MediaPlayer.Volume = _fadeEffect.GetVolume();
                }
                else
                {
                    _isFading = false;
                }
            }

            base.Update(gameTime);
        }

        // Pauses all music and sound if disabled, resumes if enabled.
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Paused)
                    {
                        _playingSounds[i].Resume();
                    }
                }

                if (!_isMusicPaused)
                {
                    MediaPlayer.Resume();
                }
            }
            else
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Playing)
                    {
                        _playingSounds[i].Pause();
                    }
                }

                MediaPlayer.Pause();
            }

            base.OnEnabledChanged(sender, args);
        }

        // Acquires an open sound slot.
        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        #region MusicFadeEffect
        private struct MusicFadeEffect
        {
            public float SourceVolume;
            public float TargetVolume;

            private TimeSpan _time;
            private TimeSpan _duration;

            public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
            {
                SourceVolume = sourceVolume;
                TargetVolume = targetVolume;
                _time = TimeSpan.Zero;
                _duration = duration;
            }

            public bool Update(TimeSpan time)
            {
                _time += time;

                if (_time >= _duration)
                {
                    _time = _duration;
                    return true;
                }

                return false;
            }

            public float GetVolume()
            {
                return MathHelper.Lerp(SourceVolume, TargetVolume, (float)_time.Ticks / _duration.Ticks);
            }
        }
        #endregion
    }
}
