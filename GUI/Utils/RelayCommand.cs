using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.Utils
{
	/// <summary>
	/// Used to bind actions to the view from inside the view model.
	/// </summary>
	internal class RelayCommand : ICommand
	{
		private readonly Predicate<object> canExecute;
		private readonly Action<object> action;

		/// <summary>
		/// Creates an instance of RelayCommand.
		/// </summary>
		/// <param name="action">The command action</param>
		public RelayCommand(Action<object> action)
			: this(action, null)
		{
		}

		/// <summary>
		/// Creates an instance of RelayCommand.
		/// </summary>
		/// <param name="action">The command action</param>
		/// <param name="canExecute">A predicate determining if the action can be executed</param>
		public RelayCommand(Action<object> action, Predicate<object> canExecute)
		{
			if (action == null)
			{
				throw new ArgumentNullException("The action must not be null.");
			}

			this.action = action;
			this.canExecute = canExecute;
		}

		#region ICommand implementation

		/// <summary>
		/// Raised whenever the executability of the underlying
		/// command changes.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		/// <summary>
		/// Determines whether the command can be executed.
		/// </summary>
		/// <param name="parameter">Additional information</param>
		/// <returns>True, if the command is executable; false otherwise</returns>
		public bool CanExecute(object parameter)
		{
			return canExecute == null ? true : canExecute(parameter);
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameter">Additional information</param>
		public void Execute(object parameter)
		{
			action(parameter);
		}

		#endregion
	}
}