using System;
using System.Net;
using System.Threading.Tasks;
using ArchiSteamFarm;
using ArchiSteamFarm.IPC.Controllers.Api;
using ArchiSteamFarm.IPC.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ArchiSteamFarm.CustomPlugins.Setu
{
	[Route("/Api/Setu")]
	public sealed class SetuController : ArchiController
	{
		/// <summary>
		///     Fetches URL of a random setu picture.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(GenericResponse<string>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(GenericResponse), (int)HttpStatusCode.ServiceUnavailable)]
		public async Task<ActionResult<GenericResponse>> CatGet()
		{
			if (ASF.WebBrowser == null)
			{
				throw new InvalidOperationException(nameof(ASF.WebBrowser));
			}

			string? link = await SetuAPI.GetRandomSetuURL(ASF.WebBrowser).ConfigureAwait(false);

			return !string.IsNullOrEmpty(link) ? Ok(new GenericResponse<string>(link)) : StatusCode((int)HttpStatusCode.ServiceUnavailable, new GenericResponse(false));
		}
	}
}
