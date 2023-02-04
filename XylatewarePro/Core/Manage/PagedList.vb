
Namespace Core.Manage
    Public Class PagedList(Of T)
        Inherits List(Of T)

        Public Property Source As IQueryable(Of T) = Nothing

        Public Property CurrentPage As Integer
        Public Property PageSize As Integer
        Public Property TotalPages As Integer
        Public Property TotalItems As Integer

        Public ReadOnly Property HasPrevious As Boolean
            Get
                Return (CurrentPage > 1)
            End Get
        End Property

        Public ReadOnly Property HasNext As Boolean
            Get
                Return (CurrentPage < TotalPages)
            End Get
        End Property

        Public Sub New(ByVal items As List(Of T), ByVal pageNumber As Integer, ByVal pageSize As Integer)
            CurrentPage = pageNumber
            pageSize = pageSize
            AddRange(items)
        End Sub

        Public Shared Function Create(ByVal SourceEx As IQueryable(Of T), ByVal pageNumber As Integer, ByVal pageSize As Integer) As PagedList(Of T)
            Dim items = SourceEx.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList
            Dim PageListEx As New PagedList(Of T)(items, pageNumber, pageSize)
            PageListEx.Source = SourceEx
            PageListEx.TotalItems = SourceEx.Count
            PageListEx.TotalPages = Math.Round(SourceEx.Count / pageSize) 'CInt(Math.Ceiling(items.Count / CDbl(pageSize)))
            Return PageListEx
        End Function

        Public Function GenerateNewPage(ByVal pageNumber As Integer) As PagedList(Of T)
            Dim items = Source.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList
            Dim PageListEx As New PagedList(Of T)(items, pageNumber, PageSize)
            PageListEx.Source = Source
            PageListEx.TotalItems = Source.Count
            PageListEx.TotalPages = Math.Round(Source.Count / PageSize) 'CInt(Math.Ceiling(items.Count / CDbl(pageSize)))
            Return PageListEx
        End Function


    End Class
End Namespace
