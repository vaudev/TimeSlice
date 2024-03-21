﻿using BlazorCalendar;
using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TimeSlice.WebApp.Components.Pages.DayView
{
	public partial class DayView : CalendarBase
	{
		[CascadingParameter( Name = "FirstDate" )]
		public DateTime Date { get; set; }

		[CascadingParameter( Name = "TasksList" )]
		public Tasks[]? TasksList { get; set; }

		[Parameter]
		public EventCallback<int> OutsideCurrentMonthClick { get; set; }

		[Parameter]
		public EventCallback<ClickEmptyDayParameter> DayClick { get; set; }

		[Parameter]
		public EventCallback<ClickEmptyDayParameter> EmptyDayClick { get; set; }

		[Parameter]
		public EventCallback<ClickTaskParameter> TaskClick { get; set; }

		[Parameter]
		public EventCallback<DragDropParameter> DragStart { get; set; }

		[Parameter]
		public EventCallback<DragDropParameter> DropTask { get; set; }

		private Tasks? TaskDragged;
		private async Task HandleDragStart( int taskID )
		{
			TaskDragged = new Tasks()
			{
				ID = taskID
			};

			DragDropParameter dragDropParameter = new()
			{
				taskID = TaskDragged.ID
			};

			await DragStart.InvokeAsync( dragDropParameter );
		}

		private async Task HandleDayOnDrop( DateTime day )
		{
			if (!Draggable)
				return;

			if (TaskDragged is null)
				return;

			DragDropParameter dragDropParameter = new()
			{
				Day = day,
				taskID = TaskDragged.ID
			};

			await DropTask.InvokeAsync( dragDropParameter );

			TaskDragged = null;
		}

		private string GetBackground( DateTime day )
		{
			int d = (int)day.DayOfWeek;

			if (d == 6)
			{
				return $"background:{SaturdayColor}";
			}
			else if (d == 0)
			{
				return $"background:{SundayColor}";
			}

			return $"background:{WeekDaysColor}";
		}


		private async Task ClickDayInternal( MouseEventArgs e, DateTime day )
		{
			if (!DayClick.HasDelegate)
				return;

			ClickEmptyDayParameter clickEmptyDayParameter = new()
			{
				Day = day,
				X = e.ClientX,
				Y = e.ClientY
			};

			await DayClick.InvokeAsync( clickEmptyDayParameter );
		}

		private async Task ClickTaskInternal( MouseEventArgs e, int taskID, DateTime day )
		{
			if (!TaskClick.HasDelegate)
				return;

			List<int> listID = new()
		{
			taskID
		};

			ClickTaskParameter clickTaskParameter = new()
			{
				IDList = listID,
				X = e.ClientX,
				Y = e.ClientY,
				Day = day
			};

			await TaskClick.InvokeAsync( clickTaskParameter );
		}
	}
}
