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
