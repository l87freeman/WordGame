namespace Game.ConsoleUI.Game.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using global::Game.ConsoleUI.Infrastructure;
    using global::Game.ConsoleUI.Interfaces.Services;
    using Infrastructure.Helpers;
    using Microsoft.Extensions.Options;
    using Models;
    using Newtonsoft.Json;
    using Serilog;

    public class BackupService : BaseServiceWithLogger<BackupService>, IBackupService
    {
        private const string BackUpFileName = "GameBackup.bk";
        
        private readonly string backupFilePath;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public BackupService(ILogger logger, IOptions<GameConfiguration> options) : base(logger)
        {
            this.backupFilePath = Path.Combine(options.Value.StorageFolder, BackUpFileName);
        }

        public bool TryRestoreGame(out GameState gameState)
        {
            var restored = false;
            gameState = null;
            var storedGame = FileHelpers.FileReaderBorrower(this.backupFilePath);
            if (storedGame != null && this.TryParseFrom(storedGame, out gameState))
            {
                var challengeId = gameState.CurrentChallenge?.Id;
                gameState.CurrentChallenge =
                    gameState.Challenges.FirstOrDefault(ch => ch.Id == challengeId);

                restored = true;
            }

            return restored;
        }

        private bool TryParseFrom(string storedGame, out GameState gameState)
        {
            var parsed = false;
            gameState = null;
            try
            {
                gameState = JsonConvert.DeserializeObject<GameState>(storedGame, this.serializerSettings);
                parsed = true;
            }
            catch (Exception e)
            {
                this.Logger.Error(e, "Was not able to parse game state");
            }

            return parsed;
        }

        public bool TryStoreGame(GameState gameState)
        {
            var stored = false;
            try
            {
                var serializedState = JsonConvert.SerializeObject(gameState, this.serializerSettings);
                FileHelpers.WriteToFile(this.backupFilePath, serializedState);

                stored = true;
            }
            catch (Exception e)
            {
                this.Logger.Error(e, "Was not able to store game state");
            }

            return stored;
        }
    }
}