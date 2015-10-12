using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Utils
{
	/// <summary>
	/// Abstract base class for view models which already implements
	/// the INotifyPropertyChanged interface.
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		/// <summary>
		/// Raised whenever a data-bound property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the PropertyChanged event for a certain property.
		/// </summary>
		/// <param name="propertyName">The name of the changed property</param>
		protected void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Sets the value of a property and raises the PropertyChanged event
		/// if the value actually changed.
		/// </summary>
		/// <typeparam name="T">The return type of the property</typeparam>
		/// <param name="backingField">The backing field of the property</param>
		/// <param name="newValue">The new value</param>
		/// <param name="propertyExpression">An expression that returns the property (e.g. '() => Foo')</param>
		protected void Set<T>(ref T backingField, T newValue, Expression<Func<T>> propertyExpression)
		{
			bool valueHasChanged = !EqualityComparer<T>.Default.Equals(backingField, newValue);
			if (valueHasChanged)
			{
				backingField = newValue;
				OnPropertyChanged(GetPropertyName(propertyExpression));
			}
		}

		/// <summary>
		/// Extracts the property name from a member expression.
		/// </summary>
		/// <typeparam name="T">The return type of the property</typeparam>
		/// <param name="propertyExpression">An expression that returns the property</param>
		/// <returns>The name of the property</returns>
		private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			var memberExpression = propertyExpression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("The expression has to be an instance of MemberExpression.", nameof(propertyExpression));
			}

			return memberExpression.Member.Name;
		}
	}
}