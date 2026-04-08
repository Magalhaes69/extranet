window.initializeLitepicker = (elementId, dotnetRef, format) => {
    const el = document.getElementById(elementId);

    if (!el) return;

    const picker = new Litepicker({
        element: el,
        format: format || 'DD/MM/YYYY',
        singleMode: true,
        autoApply: true,
        dropdowns: {
            minYear: 1999,
            maxYear: Date.now.years,
            months: true,           
            years: true             
        },

        setup: (picker) => {
            picker.on('selected', (date) => {
               
                if (!dotnetRef) {
                    console.error("dotnetRef became NULL!");
                    return;
                }

                picker.on('selected', (date) => {
                    dotnetRef.invokeMethodAsync(
                        'OnDateSelected',
                        date.format('YYYY-MM-DD')
                    );
                });

                
                el.addEventListener('input', () => {
                    if (!el.value) {
                        picker.clearSelection();
                        dotnetRef.invokeMethodAsync('OnDateCleared');
                    }
                });
            });
        }
    });
};