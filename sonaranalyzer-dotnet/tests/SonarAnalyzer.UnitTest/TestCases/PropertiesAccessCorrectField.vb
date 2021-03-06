﻿Imports System

Namespace Tests.Diagnostics
    Class NonCompliantClass_FromRspec
        Private x_ As Integer
        Private y_ As Integer

        Public Property X As Integer
            Get
                Return x_
            End Get
            Set(ByVal value As Integer)
                x_ = value
            End Set
        End Property

        Public Property Y As Integer
            Get
                Return x_ ' Noncompliant {{Refactor this getter so that it actually refers to the field 'y_'.}}
            End Get
            Set(ByVal value As Integer)
                x_ = value ' Noncompliant {{Refactor this setter so that it actually refers to the field 'y_'.}}
            End Set
        End Property
    End Class

    Structure NonCompliantStruct_FromRspec
        Private x As Integer
        Private y As Integer

        Public Property X As Integer
            Get
                Return x
            End Get
            Set(ByVal value As Integer)
                x = value
            End Set
        End Property

        Public Property Y As Integer
            Get
                Return x
            End Get
            Set(ByVal value As Integer)
                x = value
            End Set
        End Property
    End Structure

    Class NonCompliant_UnderscoresInNamesAndCasing
        Private yyy As Integer
        Private __x__X As Integer

        Public Property XX As Integer ' test that underscores and casing in names are ignored
            Get
                Return yyy ' Noncompliant {{Refactor this getter so that it actually refers to the field '__x__X'.}}
            End Get
            Set(ByVal value As Integer)
                yyy = value ' Noncompliant {{Refactor this setter so that it actually refers to the field '__x__X'.}}
            End Set
        End Property

        Public ReadOnly Property _Y___Y_Y_ As String
            Get
                Return __x__X ' Noncompliant
            End Get
        End Property
    End Class

    Class NonCompliant_FieldTypeIsIgnored
        Private aaa As Integer
        Private aString As String

        Public Property AAA As String
            Get
                Return aString ' Noncompliant - field called 'aaa' exists, even though type is different
            End Get
            Set(ByVal value As String)
                aString = value ' Noncompliant
            End Set
        End Property
    End Class

    Class NonCompliant_AssigningToExpression
        Private aaa As Integer
        Private aString As String

        Public WriteOnly Property AAA As String
            Set(ByVal value As String)
                aString = "foo" & value ' Noncompliant
            End Set
        End Property
    End Class

    Partial Class NonCompliant_PartialClass
        Private myProperty As Object
    End Class

    Partial Class NonCompliant_PartialClass
        Private anotherObject As Object
    End Class

    Partial Class NonCompliant_PartialClass
        Public Property MyProperty As Object
            Get
                Return Me.anotherObject ' Noncompliant
            End Get
            Set(ByVal value As Object)
                Me.anotherObject = value ' Noncompliant
            End Set
        End Property
    End Class

    Class NonCompliant_ComplexProperty
        Private field1_ As Integer
        Private field2_ As Integer
        Private initialized As Boolean
        Private isDisposed As Boolean

        Public Property Field1 As Integer
            Get

                If Not Me.initialized Then
                    Throw New InvalidOperationException()
                End If

                If Me.isDisposed Then
                    Throw New ObjectDisposedException("object name")
                End If

                Return Me.field2_ ' Noncompliant
'                         ^^^^^^^
            End Get
            Set(ByVal value As Integer)

                If value < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(value))
                End If

                Me.field2_ = value ' Noncompliant
'                  ^^^^^^^
            End Set
        End Property
    End Class

    Class NonCompliant_Parentheses
        Private field1_ As Integer
        Private field2_ As Integer

        Public ReadOnly Property Field2 As Integer
            Get
                Return (((Me.field1_))) ' Noncompliant
'                            ^^^^^^^
            End Get
        End Property
    End Class

    Class NonCompliant_OuterClass
        Private fielda_ As String
        Private fieldb_ As String

        Structure NonCompliant_NestedClass
            Private fielda_ As Integer
            Private fieldb_ As Integer

            Public Property FieldA As Integer
                Get
                    Return Me.fieldb_  ' Noncompliant
                End Get
                Set(ByVal value As Integer)
                    Me.fieldb_ = value ' Noncompliant
                End Set
            End Property
        End Structure
    End Class

    Structure Compliant_Indexer
        ' Declare an array to store the data elements.
        Private ReadOnly arr As Integer() = New Integer(99) {}

        ' Define the indexer to allow client code to use [] notation.
        Default Public Property Item(ByVal i As Integer) As Integer
            Get
                Return arr(i) ' Compliant - we don't know which field to check against
            End Get
            Set(ByVal value As Integer)
                arr(i) = value
            End Set
        End Property
    End Structure

    Class CompliantClass
        Private xxx_ As Integer

        Public Property XXX As Integer
            Get
                Return xxx_
            End Get
            Set(ByVal value As Integer)
                xxx_ = value
            End Set
        End Property

        Public Property UUU As Integer
            Get
                Return xxx_ ' Compliant - no matching field name
            End Get
            Set(ByVal value As Integer)
                xxx_ = value
            End Set
        End Property

        Private _a_b_c As String
        Private abc As String
        Private yyy As String

        Public Property Abc As String
            Get
                Return yyy ' Compliant - multiple possible matching field names, so don't raise
            End Get
            Set(ByVal value As String)
                yyy = value
            End Set
        End Property
    End Class

    Class Compliant_ImplicitProperties
        Private firstName As String
        Private secondName As String
        Public Property FirstName As String = "Jane"
        Public Property SecondName As String
    End Class

    Class WrappedClass
        Friend field1 As Integer
        Friend field2 As Integer
    End Class

    Class Compliant_WrappedObject
        Private wrapped As WrappedClass

        Public Property Field2 As Integer
            Get
                Return wrapped.field1
            End Get
            Get
                wrapped.field1 = value
            End Get
        End Property
    End Class

    Class BaseClass
        Protected field1 As Integer
    End Class

    Class ChildClass
        Inherits BaseClass

        Private field2 As Integer

        Public Property Field1 As Integer
            Get
                Return field2 ' Compliant - aren't checking inherited fields
            End Get
            Set(ByVal value As Integer)
                field2 = value
            End Set
        End Property
    End Class
End Namespace
