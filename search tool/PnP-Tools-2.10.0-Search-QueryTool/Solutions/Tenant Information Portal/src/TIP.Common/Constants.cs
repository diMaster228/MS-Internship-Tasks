﻿// ------------------------------------------------------------------------------
//The MIT License(MIT)

//Copyright(c) 2015 Office Developer
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
// ------------------------------------------------------------------------------

namespace TIP.Common
{
	/// <summary>
	/// Static member to hold constants
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Used to identify the key which is either in the .config file or Azure config
		/// </summary>
		public static class Configuration
		{
			public const string CLIENT_ID_KEY              = "ida:ClientId";
			public const string CLIENT_SECRET_KEY          = "ida:ClientSecret";
			public const string POST_LOGOUTREDIRECTURI_KEY = "ida:PostLogoutRedirectUri";
			public const string TENANT_KEY                 = "ida:Tenant";
			public const string CONNECTOR_URL_KEY          = "ConnectorUrl";
			public const string PORTAL_URL_KEY             = "PortalUrl";
			public const string NOTIFICATION_INTERVAL_KEY  = "NotificationInterval";
		}

		public static class ErrorCodes
		{
			public const string GENERAL = "TIP50000";
		}

	}
}
