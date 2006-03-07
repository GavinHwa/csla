Imports System.Text.RegularExpressions

Namespace Validation

  ''' <summary>
  ''' Implements common business rules.
  ''' </summary>
  Public Module CommonRules

#Region " StringRequired "

    ''' <summary>
    ''' Rule ensuring a String value contains one or more
    ''' characters.
    ''' </summary>
    ''' <param name="target">Object containing the data to validate</param>
    ''' <param name="e">Arguments parameter specifying the name of the String
    ''' property to validate</param>
    ''' <returns>False if the rule is broken</returns>
    ''' <remarks>
    ''' This implementation uses late binding, and will only work
    ''' against String property values.
    ''' </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
    Public Function StringRequired( _
      ByVal target As Object, ByVal e As RuleArgs) As Boolean

      Dim value As String = _
        CStr(CallByName(target, e.PropertyName, CallType.Get))
      If Len(value) = 0 Then
        e.Description = _
          String.Format(My.Resources.StringRequiredRule, e.PropertyName)
        Return False

      Else
        Return True
      End If

    End Function

#End Region

#Region " StringMaxLength "

    ''' <summary>
    ''' Rule ensuring a String value doesn't exceed
    ''' a specified length.
    ''' </summary>
    ''' <param name="target">Object containing the data to validate</param>
    ''' <param name="e">Arguments parameter specifying the name of the String
    ''' property to validate</param>
    ''' <returns>False if the rule is broken</returns>
    ''' <remarks>
    ''' This implementation uses late binding, and will only work
    ''' against String property values.
    ''' </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
    Public Function StringMaxLength(ByVal target As Object, _
      ByVal e As RuleArgs) As Boolean

      Dim max As Integer = DirectCast(e, MaxLengthRuleArgs).MaxLength
      If Len(CallByName( _
          target, e.PropertyName, CallType.Get).ToString) > max Then
        e.Description = _
          String.Format(My.Resources.StringMaxLengthRule, e.PropertyName, max)
        Return False
      Else
        Return True
      End If
    End Function

    Public Class MaxLengthRuleArgs
      Inherits RuleArgs

      Private mMaxLength As Integer

      Public ReadOnly Property MaxLength() As Integer
        Get
          Return mMaxLength
        End Get
      End Property

      Public Sub New(ByVal propertyName As String, ByVal maxLength As Integer)
        MyBase.New(propertyName)
        mMaxLength = maxLength
      End Sub

      ''' <summary>
      ''' Returns a string representation of the object.
      ''' </summary>
      Public Overrides Function ToString() As String
        Return MyBase.ToString & "!" & mMaxLength.ToString
      End Function

    End Class

#End Region

#Region " IntegerMaxValue "

    Public Function IntegerMaxValue(ByVal target As Object, ByVal e As RuleArgs) As Boolean
      Dim max As Integer = CType(e, IntegerMaxValueRuleArgs).MaxValue
      Dim value As Integer = CType(CallByName(target, e.PropertyName, CallType.Get), Integer)
      If value > max Then
        e.Description = String.Format(My.Resources.MaxValueRule, _
          e.PropertyName, max.ToString)
        Return False
      Else
        Return True
      End If
    End Function

    Public Class IntegerMaxValueRuleArgs
      Inherits RuleArgs

      Private mMaxValue As Integer

      Public ReadOnly Property MaxValue() As Integer
        Get
          Return mMaxValue
        End Get
      End Property

      Public Sub New(ByVal propertyName As String, ByVal maxValue As Integer)
        MyBase.New(propertyName)
        mMaxValue = maxValue
      End Sub

      ''' <summary>
      ''' Returns a string representation of the object.
      ''' </summary>
      Public Overrides Function ToString() As String
        Return MyBase.ToString & "!" & mMaxValue.ToString
      End Function

    End Class

#End Region

#Region " MaxValue "

    Public Function MaxValue(Of T)(ByVal target As Object, ByVal e As RuleArgs) As Boolean

      Dim max As Object = CType(e, MaxValueRuleArgs(Of T)).MaxValue
      Dim value As Object = CallByName(target, e.PropertyName, CallType.Get)
      Dim result As Boolean
      Dim pType As Type = GetType(T)
      If pType.IsPrimitive Then
        If pType.Equals(GetType(Integer)) Then
          result = (CInt(value) <= CInt(max))

        ElseIf pType.Equals(GetType(Boolean)) Then
          result = (CBool(value) <= CBool(max))

        ElseIf pType.Equals(GetType(Single)) Then
          result = (CSng(value) <= CSng(max))

        ElseIf pType.Equals(GetType(Double)) Then
          result = (CDbl(value) <= CDbl(max))

        ElseIf pType.Equals(GetType(Byte)) Then
          result = (CByte(value) <= CByte(max))

        ElseIf pType.Equals(GetType(Char)) Then
          result = (CChar(value) <= CChar(max))

        ElseIf pType.Equals(GetType(Short)) Then
          result = (CShort(value) <= CShort(max))

        ElseIf pType.Equals(GetType(Long)) Then
          result = (CLng(value) <= CLng(max))

        ElseIf pType.Equals(GetType(UShort)) Then
          result = (CUShort(value) <= CUShort(max))

        ElseIf pType.Equals(GetType(UInteger)) Then
          result = (CUInt(value) <= CUInt(max))

        ElseIf pType.Equals(GetType(ULong)) Then
          result = (CULng(value) <= CULng(max))

        ElseIf pType.Equals(GetType(SByte)) Then
          result = (CSByte(value) <= CSByte(max))

        Else
          Throw New ArgumentException(My.Resources.PrimitiveTypeRequired)
        End If

      Else  ' not primitive
        Throw New ArgumentException(My.Resources.PrimitiveTypeRequired)
      End If

      If Not result Then
        e.Description = String.Format(My.Resources.MaxValueRule, _
          e.PropertyName, max.ToString)
        Return False

      Else
        Return True
      End If

    End Function

    Public Class MaxValueRuleArgs(Of T)
      Inherits RuleArgs

      Private mMaxValue As T

      Public ReadOnly Property MaxValue() As T
        Get
          Return mMaxValue
        End Get
      End Property

      Public Sub New(ByVal propertyName As String, ByVal maxValue As T)
        MyBase.New(propertyName)
        mMaxValue = maxValue
      End Sub

      ''' <summary>
      ''' Returns a string representation of the object.
      ''' </summary>
      Public Overrides Function ToString() As String
        Return MyBase.ToString & "!" & mMaxValue.ToString
      End Function

    End Class

#End Region

#Region " MinValue "

    Public Function MinValue(Of T)(ByVal target As Object, ByVal e As RuleArgs) As Boolean

      Dim min As Object = CType(e, MinValueRuleArgs(Of T)).MinValue
      Dim value As Object = CallByName(target, e.PropertyName, CallType.Get)
      Dim result As Boolean
      Dim pType As Type = GetType(T)
      If pType.IsPrimitive Then
        If pType.Equals(GetType(Integer)) Then
          result = (CInt(value) >= CInt(min))

        ElseIf pType.Equals(GetType(Boolean)) Then
          result = (CBool(value) >= CBool(min))

        ElseIf pType.Equals(GetType(Single)) Then
          result = (CSng(value) >= CSng(min))

        ElseIf pType.Equals(GetType(Double)) Then
          result = (CDbl(value) >= CDbl(min))

        ElseIf pType.Equals(GetType(Byte)) Then
          result = (CByte(value) >= CByte(min))

        ElseIf pType.Equals(GetType(Char)) Then
          result = (CChar(value) >= CChar(min))

        ElseIf pType.Equals(GetType(Short)) Then
          result = (CShort(value) >= CShort(min))

        ElseIf pType.Equals(GetType(Long)) Then
          result = (CLng(value) >= CLng(min))

        ElseIf pType.Equals(GetType(UShort)) Then
          result = (CUShort(value) >= CUShort(min))

        ElseIf pType.Equals(GetType(UInteger)) Then
          result = (CUInt(value) >= CUInt(min))

        ElseIf pType.Equals(GetType(ULong)) Then
          result = (CULng(value) >= CULng(min))

        ElseIf pType.Equals(GetType(SByte)) Then
          result = (CSByte(value) >= CSByte(min))

        Else
          Throw New ArgumentException(My.Resources.PrimitiveTypeRequired)
        End If

      Else  ' not primitive
        Throw New ArgumentException(My.Resources.PrimitiveTypeRequired)
      End If

      If Not result Then
        e.Description = String.Format(My.Resources.MinValueRule, _
          e.PropertyName, min.ToString)
        Return False

      Else
        Return True
      End If

    End Function

    Public Class MinValueRuleArgs(Of T)
      Inherits RuleArgs

      Private mMinValue As T

      Public ReadOnly Property MinValue() As T
        Get
          Return mMinValue
        End Get
      End Property

      Public Sub New(ByVal propertyName As String, ByVal minValue As T)
        MyBase.New(propertyName)
        mMinValue = minValue
      End Sub

      ''' <summary>
      ''' Returns a string representation of the object.
      ''' </summary>
      Public Overrides Function ToString() As String
        Return MyBase.ToString & "!" & mMinValue.ToString
      End Function

    End Class

#End Region

#Region " RegEx "

    ''' <summary>
    ''' Rule that checks to make sure a value
    ''' matches a given regex pattern.
    ''' </summary>
    ''' <param name="target">Object containing the data to validate</param>
    ''' <param name="e">RegExRuleArgs parameter specifying the name of the 
    ''' property to validate and the regex pattern.</param>
    ''' <returns>False if the rule is broken</returns>
    ''' <remarks>
    ''' This implementation uses late binding.
    ''' </remarks>
    Public Function RegExMatch(ByVal target As Object, _
      ByVal e As RuleArgs) As Boolean

      Dim rx As Regex = DirectCast(e, RegExRuleArgs).RegEx
      If Not rx.IsMatch(CallByName( _
          target, e.PropertyName, CallType.Get).ToString) Then
        e.Description = _
          String.Format(My.Resources.RegExMatchRule, e.PropertyName)
        Return False
      Else
        Return True
      End If
    End Function

    Public Enum RegExPatterns
      SSN
      Email
    End Enum

    Public Class RegExRuleArgs
      Inherits RuleArgs

      Private mRegEx As Regex

      Public ReadOnly Property RegEx() As Regex
        Get
          Return mRegEx
        End Get
      End Property

      Public Sub New(ByVal propertyName As String, ByVal pattern As RegExPatterns)
        MyBase.New(propertyName)
        mRegEx = New Regex(GetPattern(pattern))
      End Sub

      Public Sub New(ByVal propertyName As String, ByVal pattern As String)
        MyBase.New(propertyName)
        mRegEx = New Regex(pattern)
      End Sub

      Public Sub New(ByVal propertyName As String, ByVal regEx As Regex)
        MyBase.New(propertyName)
        mRegEx = regEx
      End Sub

      ''' <summary>
      ''' Returns a string representation of the object.
      ''' </summary>
      Public Overrides Function ToString() As String
        Return MyBase.ToString & "!" & mRegEx.ToString
      End Function

      Public Shared Function GetPattern(ByVal pattern As RegExPatterns) As String
        Select Case pattern
          Case RegExPatterns.SSN
            Return "^\d{3}-\d{2}-\d{4}$"

          Case RegExPatterns.Email
            Return "\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b"

          Case Else
            Return ""
        End Select
      End Function

    End Class

#End Region

  End Module

End Namespace
