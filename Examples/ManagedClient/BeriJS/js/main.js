
    // рабочие данные приложения
    var dataRoot = {
        serviceUrl: "https://i.irklib.ru/Beri/Beri.ashx?", // адрес сервиса
        books: [], // перечень книг
        messageVisible: false, // сообщение видно?
        messageText: "", // текст сообщения
        ticket: "", // номер читательского билета
        email: "", // email или номер телефона читателя
        keyword: "", // ключевое слово для поиска
        buttonVisible: true, // показывать кнопку "Все книги"?
        foxVisible: false, // показывать котика?
        modalMessage: "", // сообщение в модальном окне
    };

    var app = new Vue ({
         el: '#app',
         data: dataRoot,

         computed: {
            countSelected: function() {
                return this.books.filter(book => book.selected).length;
            }
         }
    }); // new Vue

    function showFox() {
        app.foxVisible = true;
    } // showFox

    function hideFox() {
        app.foxVisible = false;
    } // hideFox

    function hideMessage() {
        app.messageVisible = false;
    } // hideMessage

    function showMessage(msgText) {
        if (msgText) {
            app.messageText = msgText;
            app.messageVisible = true;
        } else {
            app.messageVisible = false;
        }
    } // showMessage

    function showModal(msgText) {
        app.modalMessage = msgText;
        $('#bookModal').modal();
    } // showModal

    function retrieveBooks(url, after = null) {
        showFox();
        $.getJSON(url, function (books) {
            books.forEach(function (book) {
                app.books.push(book);
            });
        })
            .always(function () {
                if (after) {
                    after();
                }
                hideFox();
            });
    } // retrieveBooks

    function allBooks() {
        window.scroll(0, 0);
        hideMessage();
        app.books = [];
        const url = app.serviceUrl + 'all';
        retrieveBooks(url);
        app.buttonVisible = false;
    } // allBooks

    function randomBooks() {
        const url = app.serviceUrl + 'random';
        retrieveBooks(url, function () {
            const url2 = app.serviceUrl + 'count';
            $.getJSON(url2, function (data) {
                const message = 'Всего в каталоге проекта '
                    + data + ' книг. Из них показано случайных книг: '
                    + app.books.length;
                showMessage(message);
            })
        });
    } // randomBooks

    function searchBooks() {
        window.scroll(0, 0);
        hideMessage();
        if (!app.keyword) {
            return false;
        }

        app.books = [];
        const url = app.serviceUrl + 'search=' + app.keyword;
        console.log(url);
        retrieveBooks(url, function () {
            if (app.books.length === 0) {
                showMessage('По Вашему запросу ничего не найдено');
            }
            $('.navbar-toggler').click();
        });
        return false;
    } // searchBooks

    function showBacket() {
        hideMessage();
        const backet = app.books.filter(book => book.selected);
        if (backet.length === 0) {
            showModal('Корзина пуста. Вы не отметили ни одной книги.');
        } else {
            app.books = backet;
        }
        return false;
    } // showBacket

    function orderBooks() {
        const ticket = app.ticket.trim();
        if (!ticket) {
            showModal("Не указан читательский!")
            return false;
        }

        const email = app.email.trim();
        if (!email) {
            showModal("Не указан e-mail!")
            return false;
        }

        const selectedBooks = app.books.filter(book => book.selected);
        if (!selectedBooks.length) {
            showModal("Не выбраны книги!");
            return false;
        }

        let url = app.serviceUrl + 'order=';
        selectedBooks.forEach(function (item) {
            url = url + item.mfn + ",";
        });
        url = url
            + '&ticket=' + ticket
            + '&email=' + email;

        showFox();
        $.getJSON(url, function (data) {
            hideFox();
            if (data.ok) {
                window.location = "ThankYou.html";
            } else {
                showModal(data.message);
                // showMessage(data.message);
            }
        })

        return false;
    } // requestBooks

    function detectReader() {
        // app.ticket = window.location.search.substr(1);
        var url = new URL(window.location.href);
        var parameters = url.searchParams;
        app.ticket = parameters.get("id");
    } // detectReader

    function enableTooltips() {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });
    } // enableTooltips

    // сначала показываем случайные книги
    randomBooks();
    detectReader();
    enableTooltips();

