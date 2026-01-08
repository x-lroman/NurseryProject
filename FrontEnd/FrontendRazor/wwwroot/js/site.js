document.addEventListener('DOMContentLoaded', function () {
    initializeSidebar();
    initializeAlerts();
    initializeTableSearch();
});

// =============================================
// Gestión del Sidebar
// =============================================
function initializeSidebar() {
    const menuToggle = document.getElementById('menuToggle');
    const mobileMenuToggle = document.getElementById('mobileMenuToggle');
    const sidebar = document.getElementById('sidebar');

    if (menuToggle && sidebar) {
        menuToggle.addEventListener('click', function () {
            sidebar.classList.toggle('closed');
        });
    }

    if (mobileMenuToggle && sidebar) {
        mobileMenuToggle.addEventListener('click', function () {
            sidebar.classList.toggle('mobile-open');
        });

        // Cerrar menú al hacer clic en un enlace
        const navItems = sidebar.querySelectorAll('.nav-item');
        navItems.forEach(item => {
            item.addEventListener('click', function () {
                if (window.innerWidth <= 768) {
                    sidebar.classList.remove('mobile-open');
                }
            });
        });

        // Cerrar menú al hacer clic fuera
        document.addEventListener('click', function (event) {
            if (window.innerWidth <= 768 &&
                sidebar.classList.contains('mobile-open') &&
                !sidebar.contains(event.target) &&
                !mobileMenuToggle.contains(event.target)) {
                sidebar.classList.remove('mobile-open');
            }
        });
    }
}

// =============================================
// Auto-ocultar alertas
// =============================================
function initializeAlerts() {
    const alerts = document.querySelectorAll('.alert');

    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.transition = 'opacity 0.5s ease';
            alert.style.opacity = '0';

            setTimeout(() => {
                alert.remove();
            }, 500);
        }, 5000);
    });
}

// =============================================
// Búsqueda en Tablas
// =============================================
function initializeTableSearch() {
    const searchInput = document.getElementById('searchInput');
    const table = document.querySelector('.data-table');

    if (!searchInput || !table) return;

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase();
        const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        let visibleCount = 0;

        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const text = row.textContent.toLowerCase();

            if (text.includes(searchTerm)) {
                row.style.display = '';
                visibleCount++;
            } else {
                row.style.display = 'none';
            }
        }

        const recordCount = document.getElementById('recordCount');
        if (recordCount) {
            recordCount.textContent = visibleCount;
        }
    });
}

// =============================================
// Función de Eliminación
// =============================================
function deleteItem(id, controller) {
    if (!confirm('¿Estás seguro de eliminar este registro?')) {
        return;
    }

    // Obtener el token anti-forgery
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : '';

    fetch(`/${controller}/Delete/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showNotification(data.message, 'success');
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            } else {
                showNotification(data.message, 'error');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showNotification('Error al eliminar el registro', 'error');
        });
}

// Funciones específicas para cada entidad
function deleteExpediente(id) {
    deleteItem(id, 'Expedientes');
}

function deleteLote(id) {
    deleteItem(id, 'Lotes');
}

function deleteContenedor(id) {
    deleteItem(id, 'Contenedores');
}

function deletePalete(id) {
    deleteItem(id, 'Paletes');
}

function deleteCaja(id) {
    deleteItem(id, 'Cajas');
}

function deleteBandeja(id) {
    deleteItem(id, 'Bandejas');
}

function deletePlanta(id) {
    deleteItem(id, 'Plantas');
}

// =============================================
// Sistema de Notificaciones
// =============================================
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type === 'error' ? 'error' : 'success'}`;
    notification.style.position = 'fixed';
    notification.style.top = '20px';
    notification.style.right = '20px';
    notification.style.zIndex = '9999';
    notification.style.minWidth = '300px';
    notification.style.maxWidth = '500px';
    notification.style.animation = 'slideIn 0.3s ease';

    const icon = type === 'error' ? 'fa-exclamation-circle' : 'fa-check-circle';
    notification.innerHTML = `
        <i class="fas ${icon}"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 3000);
}

// =============================================
// Validación de Formularios
// =============================================
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return true;

    const requiredFields = form.querySelectorAll('[required]');
    let isValid = true;

    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            isValid = false;
            field.classList.add('is-invalid');

            // Mostrar mensaje de error
            let errorSpan = field.nextElementSibling;
            if (!errorSpan || !errorSpan.classList.contains('text-danger')) {
                errorSpan = document.createElement('span');
                errorSpan.className = 'text-danger';
                errorSpan.textContent = 'Este campo es requerido';
                field.parentNode.insertBefore(errorSpan, field.nextSibling);
            }
        } else {
            field.classList.remove('is-invalid');
        }
    });

    if (!isValid) {
        showNotification('Por favor, complete todos los campos requeridos', 'error');
    }

    return isValid;
}

// =============================================
// Formateo de Fechas
// =============================================
function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

// =============================================
// Ordenamiento de Tablas
// =============================================
function sortTable(columnIndex) {
    const table = document.querySelector('.data-table');
    if (!table) return;

    const tbody = table.getElementsByTagName('tbody')[0];
    const rows = Array.from(tbody.getElementsByTagName('tr'));
    const isAscending = table.getAttribute('data-sort-order') !== 'asc';

    rows.sort((a, b) => {
        const aValue = a.getElementsByTagName('td')[columnIndex]?.textContent.trim() || '';
        const bValue = b.getElementsByTagName('td')[columnIndex]?.textContent.trim() || '';

        if (!isNaN(aValue) && !isNaN(bValue)) {
            return isAscending ? aValue - bValue : bValue - aValue;
        }

        return isAscending
            ? aValue.localeCompare(bValue)
            : bValue.localeCompare(aValue);
    });

    rows.forEach(row => tbody.appendChild(row));
    table.setAttribute('data-sort-order', isAscending ? 'asc' : 'desc');
}

// =============================================
// Exportar a CSV
// =============================================
function exportTableToCSV(filename = 'datos.csv') {
    const table = document.querySelector('.data-table');
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    for (let i = 0; i < rows.length; i++) {
        let row = [], cols = rows[i].querySelectorAll('td, th');

        for (let j = 0; j < cols.length - 1; j++) { // Excluir columna de acciones
            let data = cols[j].innerText.replace(/(\r\n|\n|\r)/gm, '').replace(/(\s\s)/gm, ' ');
            data = data.replace(/"/g, '""');
            row.push('"' + data + '"');
        }

        csv.push(row.join(','));
    }

    downloadCSV(csv.join('\n'), filename);
}

function downloadCSV(csv, filename) {
    const csvFile = new Blob([csv], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

// =============================================
// Imprimir Tabla
// =============================================
function printTable() {
    window.print();
}

// =============================================
// Loading State
// =============================================
function showLoading(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = `
            <div style="text-align: center; padding: 2rem;">
                <i class="fas fa-spinner fa-spin fa-2x"></i>
                <p style="margin-top: 1rem;">Cargando...</p>
            </div>
        `;
    }
}

function hideLoading(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = '';
    }
}

// =============================================
// Validaciones
// =============================================
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function validateNumber(value, min = 0, max = Infinity) {
    const num = parseFloat(value);
    return !isNaN(num) && num >= min && num <= max;
}

// =============================================
// Animaciones CSS
// =============================================
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }

    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }

    .is-invalid {
        border-color: #dc3545 !important;
    }

    @media print {
        .header, .sidebar, .btn, .action-buttons, .table-controls {
            display: none !important;
        }
        .main-content {
            padding: 0 !important;
        }
        .data-table {
            width: 100% !important;
        }
    }
`;
document.head.appendChild(style);

// =============================================
// Exportar funciones globales
// =============================================
window.deleteExpediente = deleteExpediente;
window.deleteLote = deleteLote;
window.deleteContenedor = deleteContenedor;
window.deletePalete = deletePalete;
window.deleteCaja = deleteCaja;
window.deleteBandeja = deleteBandeja;
window.deletePlanta = deletePlanta;
window.showNotification = showNotification;
window.validateForm = validateForm;
window.formatDate = formatDate;
window.sortTable = sortTable;
window.exportTableToCSV = exportTableToCSV;
window.printTable = printTable;
window.showLoading = showLoading;
window.hideLoading = hideLoading;