using System;
using System.Collections.Generic;
using System.Linq;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Extensions;
using WorkManager.Models.Interfaces;
using WorkManager.Models.Interfaces.ModelServices;

namespace WorkManager.BL.Services
{
	public class RecordTotalCalculatorService : IRecordTotalCalculatorService
	{
		public double Calculate<T>(T records, DateTime from, DateTime to) where T : IEnumerable<IWorkRecordModelBase>
		{
			return records.Where(s => s.ActualDateTime.IsBetween(from, to)).Sum(s => s.CalculatedPrice);
		}

		public double CalculatePriceThisMonth<T>(T records) where T : IEnumerable<IWorkRecordModelBase>
		{
			return records.Where(s => s.ActualDateTime.Month == DateTime.Today.Month && s.ActualDateTime.Year == DateTime.Today.Year).Sum(s => s.CalculatedPrice);
		}

		public double CalculatePriceThisYear<T>(T records) where T : IEnumerable<IWorkRecordModelBase>
		{
			return records.Where(s => s.ActualDateTime.Year == DateTime.Today.Year).Sum(s => s.CalculatedPrice);
		}
	}
}