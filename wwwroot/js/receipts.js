window.downloadFileFromBytes = function (filename, contentType, bytes) {
    const blob = new Blob([new Uint8Array(bytes)], { type: contentType });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};

window.generateProviderStatementPdf = function (providerName, providerType, providerPhone, generatedDate, summary, rows) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({ orientation: 'landscape' });

    doc.setFillColor(30, 110, 194);
    doc.rect(0, 0, 297, 22, 'F');
    doc.setFontSize(16);
    doc.setFont('helvetica', 'bold');
    doc.setTextColor(255, 255, 255);
    doc.text('DiaspoDirect — Provider Statement', 148.5, 14, { align: 'center' });

    doc.setFontSize(13);
    doc.setFont('helvetica', 'bold');
    doc.setTextColor(0, 0, 0);
    doc.text(providerName, 14, 34);

    doc.setFontSize(9);
    doc.setFont('helvetica', 'normal');
    doc.setTextColor(100, 100, 100);
    doc.text(providerType + (providerPhone ? '  ·  ' + providerPhone : ''), 14, 41);
    doc.text('Generated: ' + generatedDate, 14, 47);

    const summaryItems = [
        ['Total Paid',          summary.totalPaid + ' XOF'],
        ['Total Outstanding',   summary.totalOutstanding + ' XOF'],
        ['Last Payment Date',   summary.lastPaymentDate],
        ['Avg Payment Delay',   summary.avgDelay],
    ];
    let sx = 14;
    summaryItems.forEach(function ([label, value]) {
        doc.setDrawColor(220, 220, 220);
        doc.setFillColor(248, 249, 250);
        doc.roundedRect(sx, 53, 62, 18, 2, 2, 'FD');
        doc.setFontSize(7);
        doc.setTextColor(100, 100, 100);
        doc.text(label, sx + 4, 61);
        doc.setFontSize(10);
        doc.setFont('helvetica', 'bold');
        doc.setTextColor(0, 0, 0);
        doc.text(String(value), sx + 4, 68);
        doc.setFont('helvetica', 'normal');
        sx += 66;
    });

    doc.autoTable({
        startY: 78,
        head: [['Date', 'Prescription', 'Customer', 'Amount (XOF)', 'Reference', 'Status', 'Paid On']],
        body: rows.map(function (r) {
            return [r.date, r.prescription, r.customer, r.amountXof, r.reference, r.status, r.paidOn];
        }),
        styles: { fontSize: 8, cellPadding: 3 },
        headStyles: { fillColor: [30, 110, 194], textColor: 255, fontStyle: 'bold' },
        alternateRowStyles: { fillColor: [248, 249, 250] },
        columnStyles: {
            0: { cellWidth: 24 },
            3: { halign: 'right', cellWidth: 28 },
            6: { cellWidth: 24 },
        },
    });

    const finalY = doc.lastAutoTable.finalY + 8;
    doc.setFontSize(8);
    doc.setTextColor(150, 150, 150);
    doc.text('DiaspoDirect · Confidential Provider Statement', 148.5, finalY, { align: 'center' });

    doc.save(providerName.replace(/\s+/g, '_') + '_Statement.pdf');
};

window.generateReceiptPdf = function (date, payorName, amountUsd, paymentId) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    doc.setFontSize(22);
    doc.setFont('helvetica', 'bold');
    doc.text('DiaspoDirect', 105, 22, { align: 'center' });

    doc.setFontSize(13);
    doc.setFont('helvetica', 'normal');
    doc.text('Payment Receipt', 105, 32, { align: 'center' });

    doc.setDrawColor(180, 180, 180);
    doc.setLineWidth(0.4);
    doc.line(20, 38, 190, 38);

    const rows = [
        ['Payment Date', date],
        ['Payor Name', payorName],
        ['Amount Paid (USD)', '$' + amountUsd],
        ['Payment ID', paymentId],
    ];

    let y = 56;
    doc.setFontSize(11);
    rows.forEach(function ([label, value]) {
        doc.setFont('helvetica', 'bold');
        doc.text(label + ':', 22, y);
        doc.setFont('helvetica', 'normal');
        doc.text(value, 90, y);
        y += 14;
    });

    doc.line(20, y + 4, 190, y + 4);
    doc.setFontSize(9);
    doc.setTextColor(130, 130, 130);
    doc.text('Thank you for using DiaspoDirect', 105, y + 14, { align: 'center' });

    window.open(doc.output('bloburl'), '_blank');
};
