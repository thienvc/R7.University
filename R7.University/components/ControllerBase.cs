﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using DotNetNuke.Collections;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using R7.University;

namespace R7.University
{
	public abstract class ControllerBase : ModuleSearchBase
	{
		#region Common methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Launchpad.LaunchpadController"/> class.
		/// </summary>
		public ControllerBase ()
		{ 

		}

		/// <summary>
		/// Adds a new T object into the database
		/// </summary>
		/// <param name='info'></param>
		public void Add<T> (T info) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Insert (info);
			}
		}

		/// <summary>
		/// Get single object from the database
		/// </summary>
		/// <returns>
		/// The object
		/// </returns>
		/// <param name='itemId'>
		/// Item identifier.
		/// </param>
		public T Get<T> (int itemId) where T: class
		{
			T info;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				info = repo.GetById (itemId);
			}

			return info;
		}

		/// <summary>
		/// Get single object from the database
		/// </summary>
		/// <returns>
		/// The object
		/// </returns>
		/// <param name='itemId'>
		/// Item identifier.
		/// </param>
		/// <param name='scopeId'>
		/// Scope identifier (like moduleId)
		/// </param>
		public T Get<T> (int itemId, int scopeId) where T: class
		{
			T info;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				info = repo.GetById (itemId, scopeId);
			}

			return info;
		}

		/// <summary>
		/// Updates an object already stored in the database
		/// </summary>
		/// <param name='info'>
		/// Info.
		/// </param>
		public void Update<T> (T info) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Update (info);
			}
		}

		/// <summary>
		/// Gets all objects for items matching scopeId
		/// </summary>
		/// <param name='scopeId'>
		/// Scope identifier (like moduleId)
		/// </param>
		/// <returns></returns>
		public IEnumerable<T> GetObjects<T> (int scopeId) where T: class
		{
			IEnumerable<T> infos = null;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.Get (scopeId);
				
				// Without [Scope("ModuleID")] it should be like:
				// infos = repo.Find ("WHERE ModuleID = @0", moduleId);
			}

			return infos;
		}

		/// <summary>
		/// Gets all objects of type T from database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> GetObjects<T> () where T: class
		{
			IEnumerable<T> infos = null;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.Get ();
			}

			return infos;
		}

		/// <summary>
		/// Gets the all objects of type T from result of a dynamic sql query
		/// </summary>
		/// <returns>Enumerable with objects of type T</returns>
		/// <param name="sqlCondition">SQL command condition.</param>
		/// <param name="args">SQL command arguments.</param>
		/// <typeparam name="T">Type of objects.</typeparam>
		public IEnumerable<T> GetObjects<T> (string sqlConditon, params object[] args) where T: class
		{
			IEnumerable<T> infos = null;
			
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.Find (sqlConditon, args);
			}
			
			return infos;
		}

		/// <summary>
		/// Gets the all objects of type T from result of a dynamic sql query
		/// </summary>
		/// <returns>Enumerable with objects of type T</returns>
		/// <param name="cmdType">Type of an SQL command.</param>
		/// <param name="sql">SQL command.</param>
		/// <param name="args">SQL command arguments.</param>
		/// <typeparam name="T">Type of objects.</typeparam>
		public IEnumerable<T> GetObjects<T> (System.Data.CommandType cmdType, string sql, params object[] args) where T: class
		{
			IEnumerable<T> infos = null;
			
			using (var ctx = DataContext.Instance ())
			{
				infos = ctx.ExecuteQuery<T>	(cmdType, sql, args);
			}
			
			return infos;
		}

		/// <summary>
		/// Gets one page of objects of type T
		/// </summary>
		/// <param name="scopeId">Scope identifier (like moduleId)</param>
		/// <param name="index">a page index</param>
		/// <param name="size">a page size</param>
		/// <returns>A paged list of T objects</returns>
		public IPagedList<T> GetPage<T> (int scopeId, int index, int size) where T: class
		{
			IPagedList<T> infos;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.GetPage (scopeId, index, size);
			}

			return infos;
		}

		/// <summary>
		/// Gets one page of objects of type T
		/// </summary>
		/// <param name="index">a page index</param>
		/// <param name="size">a page size</param>
		/// <returns>A paged list of T objects</returns>
		public IPagedList<T> GetPage<T> (int index, int size) where T: class
		{
			IPagedList<T> infos;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.GetPage (index, size);
			}

			return infos;
		}

		/// <summary>
		/// Delete a given item from the database by instance
		/// </summary>
		/// <param name='info'></param>
		public void Delete<T> (T info) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Delete (info);
		
			}
		}

		/// <summary>
		/// Delete a given item from the database by ID
		/// </summary>
		/// <param name='itemId'></param>
		public void Delete<T> (int itemId) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Delete (repo.GetById (itemId));
			}
		}

		/// <summary>
		/// Delete some item from the database using SQL condition
		/// </summary>
		/// <param name='sqlConditon'>SQL condition</param>
		/// <param name='args'>Optional arguments</param>
		public void Delete<T> (string sqlConditon, params object[] args) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Delete (sqlConditon, args);
			}
		}

		/*
		public T ExecReader <T> (string spName, params object [] args) where T: class
		{
			using (var ctx = DataContext.Instance ())
				return ctx.ExecuteQuery<T> (CommandType.StoredProcedure, spName, args);
		}

		public T ExecScalar <T> (string spName, params object [] args) where T: class
		{
			using (var ctx = DataContext.Instance ())
				return ctx.ExecuteQuery<T> (CommandType.StoredProcedure, spName, args);
		}
		*/

		#endregion

		#region Custom methods

		public void AddEmployee (EmployeeInfo employee, 
		                        List<OccupiedPositionInfo> occupiedPositions, List<EmployeeAchievementInfo> achievements)
		{
			using (var ctx = DataContext.Instance ())
			{
				ctx.BeginTransaction ();

				try
				{
					// add Employee
					Add<EmployeeInfo> (employee);

					// add new OccupiedPositions
					foreach (var op in occupiedPositions)
					{
						op.EmployeeID = employee.EmployeeID;
						Add<OccupiedPositionInfo> (op);
					}
					
					// add new EmployeeAchievements
					foreach (var ach in achievements)
					{
						ach.EmployeeID = employee.EmployeeID;
						Add<EmployeeAchievementInfo> (ach);
					}
				
					ctx.Commit ();
				}
				catch
				{
					ctx.RollbackTransaction ();
					throw;
				}
			}

		}

		public void UpdateEmployee (EmployeeInfo employee, 
		                           List<OccupiedPositionInfo> occupiedPositions, List<EmployeeAchievementInfo> achievements)
		{
			using (var ctx = DataContext.Instance ())
			{
				ctx.BeginTransaction ();

				try
				{
					// update Employee
					Update<EmployeeInfo> (employee);

					var occupiedPositonIDs = occupiedPositions.Select (op => op.OccupiedPositionID.ToString ());
					if (occupiedPositonIDs.Any())
					{
						Delete<OccupiedPositionInfo> (
							string.Format ("WHERE [EmployeeID] = {0} AND [OccupiedPositionID] NOT IN ({1})", 
								employee.EmployeeID, Utils.FormatList (", ", occupiedPositonIDs))); 
					}
					else
					{
						// delete all employee occupied positions 
						Delete<OccupiedPositionInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID); 
					}
					
					// add new OccupiedPositions
					foreach (var op in occupiedPositions)
					{
						// REVIEW: Do we really need to set EmployeeID here?
						op.EmployeeID = employee.EmployeeID;
						
						if (op.OccupiedPositionID <= 0)
							Add<OccupiedPositionInfo> (op);
						else
							Update<OccupiedPositionInfo> (op);
					}
					
					var employeeAchievementIDs = achievements.Select (a => a.EmployeeAchievementID.ToString ());
					if (employeeAchievementIDs.Any())
					{
						// delete those not in current list
						Delete<EmployeeAchievementInfo> (
							string.Format ("WHERE [EmployeeID] = {0} AND [EmployeeAchievementID] NOT IN ({1})", 
								employee.EmployeeID, Utils.FormatList (", ", employeeAchievementIDs))); 
					}
					else
					{
						// delete all employee achievements
						Delete<EmployeeAchievementInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID);
					}

					// add new EmployeeAchievements
					foreach (var ach in achievements)
					{
						if (ach.AchievementID != null)
						{
							// reset linked properties
							ach.Title = null;
							ach.ShortTitle = null;
							ach.AchievementType = null;
						}
						
						ach.EmployeeID = employee.EmployeeID;
						if (ach.EmployeeAchievementID <= 0)
							Add<EmployeeAchievementInfo> (ach);
						else
							Update<EmployeeAchievementInfo> (ach);
					}

					ctx.Commit ();
				}
				catch
				{
					ctx.RollbackTransaction ();
					throw;
				}
			}
		}

		#endregion
	}
}

