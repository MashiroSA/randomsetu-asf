using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using ArchiSteamFarm.Json;
using ArchiSteamFarm.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;

namespace ArchiSteamFarm.CustomPlugins.Setu
{
	[Export(typeof(IPlugin))]

	internal sealed class RandomSetu : IASF, IBot, IBotCommand, IBotConnection, IBotFriendRequest, IBotMessage, IBotModules, IBotTradeOffer
	{
		public string Name => nameof(RandomSetu);

		public Version Version => typeof(RandomSetu).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

		[JsonProperty]
		public bool CustomIsEnabledField { get; private set; } = true;

		public void OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			if (additionalConfigProperties == null)
			{
				return;
			}

			foreach (KeyValuePair<string, JToken> configProperty in additionalConfigProperties)
			{
				switch (configProperty.Key)
				{
					case nameof(RandomSetu) + "TestProperty" when configProperty.Value.Type == JTokenType.Boolean:
						bool exampleBooleanValue = configProperty.Value.Value<bool>();
						ASF.ArchiLogger.LogGenericInfo(nameof(RandomSetu) + "TestProperty boolean property has been found with a value of: " + exampleBooleanValue);

						break;
				}
			}
		}

		public async Task<string?> OnBotCommand(Bot bot, ulong steamID, string message, string[] args)
		{
			switch (args[0].ToUpperInvariant())
			{
				case "SETU":
					string? randomSetuURL = await SetuAPI.GetRandomSetuURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuURL) ? randomSetuURL : "哇哦，不好意思，好像并未寻找到色图qwq";
				default:
					return null;
			}
		}

		public void OnBotDestroy(Bot bot) { }

		public void OnBotDisconnected(Bot bot, EResult reason) { }

		public Task<bool> OnBotFriendRequest(Bot bot, ulong steamID) => Task.FromResult(true);

		public void OnBotInit(Bot bot)
		{
			bot.ArchiLogger.LogGenericInfo("机器人：" + bot.BotName + " 已经加载，并包括了插件： " + nameof(RandomSetu) + "!");
			ASF.ArchiLogger.LogGenericWarning("不稳定建构，请务必注意！");
		}

		public async void OnBotInitModules(Bot bot, IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			bot.ArchiLogger.LogGenericInfo("Pausing this bot as asked from the plugin");
			await bot.Actions.Pause(true).ConfigureAwait(false);
		}

		
		public void OnBotLoggedOn(Bot bot) { }

		public Task<string?> OnBotMessage(Bot bot, ulong steamID, string message)
		{
			if (Bot.BotsReadOnly == null)
			{
				throw new InvalidOperationException(nameof(Bot.BotsReadOnly));
			}

			if (Bot.BotsReadOnly.Values.Any(existingBot => existingBot.SteamID == steamID))
			{
				return Task.FromResult<string?>(null);
			}
			
			return Task.FromResult((string?)"");
		}

		public Task<bool> OnBotTradeOffer(Bot bot, Steam.TradeOffer tradeOffer) => Task.FromResult(bot.BotName.StartsWith("TrashBot", StringComparison.OrdinalIgnoreCase));

		public void OnLoaded()
		{
			ASF.ArchiLogger.LogGenericInfo("机器人加载插件：" + nameof(OnLoaded) + "() method 调用");
			ASF.ArchiLogger.LogGenericInfo("祝你好运！");
		}
	}
}
