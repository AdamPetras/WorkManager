using System;
using System.Collections.Generic;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Services
{
	public interface IRecordTotalCalculatorService
	{
		double Calculate<T>(T records, DateTime from, DateTime to) where T : IEnumerable<IWorkRecordModelBase>;
		double CalculateThisMonth<T>(T records) where T : IEnumerable<IWorkRecordModelBase>;
		double CalculateThisYear<T>(T records) where T : IEnumerable<IWorkRecordModelBase>;
	}
}