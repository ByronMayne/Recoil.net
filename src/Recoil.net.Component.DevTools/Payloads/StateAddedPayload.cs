﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recoil.Components.DevTools.Payloads
{
	internal record StateAddedPayload(string Key, int StoreId);
}
