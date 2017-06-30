// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication.Internal
{
    /// <summary>
    /// A cookie builder that set <see cref="CookieOptions.Path"/> to the original path base plus some scope.
    /// </summary>
    public class RequestPathCookieBuilder : CookieBuilder
    {
        /// <summary>
        /// Optional additional path that is appended to the request path base.
        /// </summary>
        protected virtual string AdditionalPath { get; }

        public override CookieOptions Build(HttpContext context, DateTimeOffset expiresFrom)
        {
            // check if the user has overridden the default value of path. If so, use that instead of our default value.
            var path = Path;
            if (path == null)
            {
                var originalPathBase = context.Features.Get<IAuthenticationFeature>()?.OriginalPathBase ?? context.Request.PathBase;
                path = originalPathBase + AdditionalPath;
            }

            var options = base.Build(context, expiresFrom);

            options.Path = path ?? "/";

            return options;
        }
    }
}
