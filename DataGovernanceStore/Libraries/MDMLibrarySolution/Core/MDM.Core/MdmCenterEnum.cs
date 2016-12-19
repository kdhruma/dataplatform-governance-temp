using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Core
{
    /*
 * Copyright 2003-2007 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */
    /**
 * This is the common base class of all Java language enumeration types.
 *
 * @author  Josh Bloch
 * @author  Neal Gafter
 * @see     Class#getEnumConstants()
 * @since   1.5
 */
    /// <summary>
    /// This is a command base class for all Polymorphic Enum type in MDMCenter.
    /// </summary>
    /// <typeparam name="T">Class Type</typeparam>
    [DataContract]
    public abstract class MDMEnum<T> : IComparable<T>
    {
    /**
     * The _name of this enum constant, as declared in the enum declaration.
     * Most programmers should use the {@link #toString} method rather than
     * accessing this field.
     */
    private String _name;

/// <summary>
/// 
/// </summary>
/// <returns></returns>
/// [DataMember]
    public String Name 
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
        }
    }

    /**
     * The ordinal of this enumeration constant (its position
     * in the enum declaration, where the initial constant is assigned
     * an ordinal of zero).
     *
     * Most programmers will have no use for this field.  It is designed
     * for use by sophisticated enum-based data structures, such as
     * {@link java.util.EnumSet} and {@link java.util.EnumMap}.
     */
    private int _ordinal;

/// <summary>
/// 
/// </summary>
/// <returns></returns>
    [DataMember]
    public int Ordinal
    {
        get
        {
            return _ordinal;
        }

        set
        {
            _ordinal = value;
        }
    }

    private String _displayName;

        /// <summary>
        /// 
        /// </summary>
     [DataMember]
    public String DisplayName
    {
        get { return _displayName; }
        set { _displayName = value; }
    }
    

/// <summary>
/// 
/// </summary>
/// <param name="ordinal"></param>
/// <param name="name"></param>
/// <param name="displayName"></param>
    protected MDMEnum(int ordinal, String name, String displayName) 
    {
        _name = name;
        _ordinal = ordinal;
        _displayName = displayName;
    }

/// <summary>
/// 
/// </summary>
/// <returns></returns>
    public override String ToString()
    {
        return _name;
    }

/// <summary>
/// 
/// </summary>
/// <param name="other"></param>
/// <returns></returns>
    public Boolean equals(object other) 
    {
        var t = other as MDMEnum<T>;

        if ( t != null)
        {
            if (this._ordinal == t._ordinal && this._name == t._name)
                return true;
        }

        return false;
    }


/// <summary>
/// 
/// </summary>
/// <returns></returns>
    protected  Object clone()  {
        throw new NotSupportedException();
    }

    /**
     * Returns the enum constant of the specified enum type with the
     * specified _name.  The _name must match exactly an identifier used
     * to declare an enum constant in this type.  (Extraneous whitespace
     * characters are not permitted.)
     *
     * @param enumType the {@code Class} object of the enum type from which
     *      to return a constant
     * @param _name the _name of the constant to return
     * @return the enum constant of the specified enum type with the
     *      specified _name
     * @throws IllegalArgumentException if the specified enum type has
     *         no constant with the specified _name, or the specified
     *         class object does not represent an enum type
     * @throws NullPointerException if {@code enumType} or {@code _name}
     *         is null
     * @since 1.5
     */



    protected  void finalize() { }


    #region IComparable<T> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
    public int CompareTo(T other)
    {
        if (other is MDMEnum<T>)
        {
            var tp = other as MDMEnum<T>;

            return string.Compare(this._name,  tp._name);
        }
        else
        {
            throw new ArgumentException();
        }
    }

    #endregion
    }

}
