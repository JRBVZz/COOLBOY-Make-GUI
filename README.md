### COOLBOY Make GUI - графический интерфейс для сборки многоигровки
Эта утилита запускает команды make из указанного Makefile, используя окружение MSYS2.

### Требования
- Естественно на ПК должен быть [COOLBOY](https://github.com/ClusterM/coolboy-multirom-builder) или [COOLGIRL](https://github.com/ClusterM/coolgirl-multirom-builder) Multirom Builder, в зависимости от того для какого картриджа вы собираете многоигровку.
- Установить [MSYS2](https://github.com/msys2/msys2-installer)
- Запустить через меню Пуск -> MSYS2 -> MINGW64. В открывшемся окне выполнить команду: pacman -S make
- Установить [.NET 9.0 Runtime или SDK](https://dotnet.microsoft.com/ru-ru/download/dotnet/9.0)
- В программе во вкладке Options указать путь до bash.exe

### Сборка многоигровки
● Выберите файл Makefile, который расположен в папке COOLBOY/COOLGIRL Multirom Builder  
● Выберите файл games.list, расположенный в папке Builder/configs  
● Выберите команду:  
□ nes20 - сборка файла с расширением .nes  
□ unif - сборка файла с расширением .unif  
□ bin - сборка файла с расширением .bin  
□ all - сборка файлов во всех форматах  
□ clean - удаление всех промежуточных файлов для сборки  
● Задайте параметры:  
□ Submapper - номер субмаппера (только для COOLBOY), для COOLGIRL снять галку  
□ Save - добавляет возможность сохранений  
□ Sound - звук в меню  
□ R.Cursor - наличие курсора справа  
□ Stars - количество звезд на фоне меню (целое число от 0 до 62)  
● При необходимости добавьте параметры в поле "Доп. параметры"  
● Нажмите "Запустить" для выполнения скрипта  

В папке COOLBOY/COOLGIRL Multirom Builder появится итоговый файл/файлы многоигровки.
