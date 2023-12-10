using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public enum OrderStatus
{
	Created,
	Approved,
	DriverOnTheWay,
	DriverIsWaiting,
	InProgress,
	Completed,
	Canceled
}