window.layoutHelper = {
    toggleSidebar: function (collapsed) {
        if (collapsed) {
            document.body.classList.add("sidebar-collapse");
        } else {
            document.body.classList.remove("sidebar-collapse");
        }
    }
};

window.preventDropdownClose = () => {
    document.querySelectorAll('.dropdown-menu .nav-link').forEach(tabLink => {
        tabLink.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    });
}

window.scrollToSection = (id) => {
    const el = document.getElementById(id);
    if (!el) return;

    const offset = 80;
    const top = el.offsetTop - offset;

    window.scrollTo({
        top: top,
        behavior: "smooth"
    });
};

window.registerScrollSpy = (dotNetHelper) => {
    window.addEventListener("scroll", () => {
        const sections = document.querySelectorAll(".section-block");
        let current = "";

        sections.forEach(section => {
            const top = section.offsetTop - 100;
            if (window.scrollY >= top) {
                current = section.id;
            }
        });

        if (current) {
            dotNetHelper.invokeMethodAsync("SetActiveSection", current);
        }
    });
};

document.querySelectorAll('[data-widget="pushmenu"]').forEach(btn => {
    btn.addEventListener('click', () => {
        const footer = document.getElementById('bottom-footer');
        const wrapper = document.querySelector('.wrapper');
        if (wrapper.classList.contains('sidebar-collapse')) {
            footer.style.left = '80px'; // collapsed sidebar width
            footer.style.width = 'calc(100% - 80px)';
        } else {
            footer.style.left = '250px'; // expanded sidebar width
            footer.style.width = 'calc(100% - 250px)';
        }
    });
});

window.ShowDialog = function () {
    document.getElementById("my-dialog").showModal();
}

window.isNearBottom = (element) => {
    return element.scrollTop + element.clientHeight >= element.scrollHeight - 50;
};



