using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;

namespace ArchiSteamFarm.CustomPlugins.Setu
{
	
	[Export(typeof(IPlugin))]
	internal sealed class RandomSetu : IASF, IBot, IBotCommand2, IBotConnection, IBotFriendRequest, IBotMessage, IBotModules, IBotTradeOffer
	{
		public string Name => nameof(RandomSetu);

		public Version Version => typeof(RandomSetu).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

		[JsonProperty]
		public bool CustomIsEnabledField { get; private set; } = true;

		public Task OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			if (additionalConfigProperties == null)
			{
				return Task.CompletedTask;
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
			return Task.CompletedTask;
		}

		public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamID = 0)
		{
			switch (args[0].ToUpperInvariant())
			{
				case "SETU":
					string? randomSetuURL = await SetuAPI.GetRandomSetuURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuURL) ? randomSetuURL : "哇哦，不好意思，好像并未寻找到色图qwq";
				case "R18":
					string? randomSetuR18URL = await SetuAPI.GetRandomSetuR18URL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuR18URL) ? randomSetuR18URL : "哇哦，不好意思，好像并未寻找到色图qwq";
				default:
					return null;
			}
		}
		public Task OnBotLoggedOn(Bot bot) => Task.CompletedTask;
		
		public Task<bool> OnBotFriendRequest(Bot bot, ulong steamID) => Task.FromResult(true);

		public Task OnBotInit(Bot bot)
		{
			bot.ArchiLogger.LogGenericInfo("欢迎您：" + bot.BotName + " 。已经加载插件 " + nameof(RandomSetu) + "!");
			ASF.ArchiLogger.LogGenericWarning("不稳定建构，请务必注意！适用于版本v5.2.2.5");
			return Task.CompletedTask;
		}

		public async Task OnBotInitModules(Bot bot, IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			bot.ArchiLogger.LogGenericInfo("Pausing this bot as asked from the plugin");
			await bot.Actions.Pause(true).ConfigureAwait(false);
		}

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

		public Task<bool> OnBotTradeOffer(Bot bot, TradeOffer tradeOffer) => Task.FromResult(bot.BotName.StartsWith("TrashBot", StringComparison.OrdinalIgnoreCase));

		public Task OnLoaded()
		{
			ASF.ArchiLogger.LogGenericInfo("机器人加载插件：" + nameof(OnLoaded) + "() method 调用");
			ASF.ArchiLogger.LogGenericInfo("祝你好运！");
			return Task.CompletedTask;
		}
		
		public Task OnBotDestroy(Bot bot) => Task.CompletedTask;
		public Task OnBotDisconnected(Bot bot, EResult reason) => Task.CompletedTask;

	}
}
