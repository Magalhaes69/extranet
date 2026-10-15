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

window.getElementPosition = (element) => {

    if (!element || typeof element.getBoundingClientRect !== "function") {
        return null;
    }
    const rect = element.getBoundingClientRect();

    return {
        top: rect.top,
        left: rect.right,
        width: rect.width,
        height: rect.height,
        right: rect.right,
        bottom: rect.bottom,
        x: rect.x,
        y: rect.y
    };
};

document.addEventListener("click", function (e) {
    window.blazorModalClickOutside?.invokeMethodAsync("CloseIfOutside");
});

window.getWindowHeight = () => {
    return window.innerHeight;
};

window.getWindowWidth = () => {
    return window.innerWidth;
};
window.getWindowSize = () => {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

// Definição do FavIcon
window.setFavicon = (url) => {
    let link = document.querySelector("link[rel*='icon']");

    if (!link) {
        link = document.createElement('link');
        link.rel = 'icon';
        document.head.appendChild(link);
    }

    link.href = url;
    
};