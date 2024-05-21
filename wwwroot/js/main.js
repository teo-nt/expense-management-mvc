function confirmDelete(expenseId) {
    if (confirm("Are you sure you want to delete this expense?")) {
        window.location.href = `/expense/delete/${expenseId}`
    }
}