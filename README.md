# Navisworks_CustomTab

## Description / Описание
> EN:

Navisworks plugin for automatic creation of custom properties tab based on txt file. This plugin finds an existing element parameter, takes its value and creates a new parameter with that value. The plugin processes all elements in selected NWC or NWD file.

#### Instruction:

1.	Run the "Custom Tab" plugin in Navisworks 2019.
2.	Select NWC or NWD file.
3.	Select the TXT file with a list of new properties. TXT file structure must be:
*	The first line is the name of the new tab;
*	Each next line defines a new property. First comes the name of the tab where the original property is specified, then the name of the original property, then the name of the new property. All names in line must be separated by tabs;
*	If the property from the TXT file is missing, new property will not be created;
*	If an element has several properties which match the definition in a TXT file, then the first property from the list will be selected, the rest will be skipped;
*	There may be blank lines between the lines with properties.
4.	Press "Старт".
5.	Select the folder to save the new file and enter the name for it.
6.	After the "Файл готов" text appears you can open the new NWD file.

Plugin overview on youtube: https://www.youtube.com/watch?v=ia8MRARQt40

> RU:

Плагин Navisworks для автоматического создания вкладки пользовательских параметров на основе txt файла. Плагин находит существующий параметр элемента, берет его значение и создает новый параметр с этим значением. Плагин обрабывает все элементы в выбранном NWC или NWD файле.

#### Инструкция по работе:

1. Запустите плагин "Custom Tab" с вкладки "Надстройки инструментов 1" в Navisworks 2019.
2. Выберите файл NWC или NWD, в котором требуется создать новую вкладку. 
3. Выберите txt файл со списком новых параметров. Структура TXT файла должна быть следующая:
* Первая строка - имя новой вкладки;
* Каждая следующая строка - новый параметр. Сначала указывается имя вкладки с исходных параметром, потом имя исходного параметра, потом имя нового параметра. Все имена в строке должны быть разделены табуляцией;
* Если параметр из txt файла отсутствует, то новый параметр создан не будет;
* Если у элемента имеются несколько исходных параметров, которые подходят для создания нового параметра, то будет выбран первый параметр из списка, остальные будут пропущены;
* Между строками с параметрами могут быть пустые строки.
4. Нажмите "Старт".
5. Укажите папку для сохранения нового NWD файла с созданной вкладкой и имя нового файла.
6. После появления текста "Файл готов" можно открывать новый NWD файл.

Обзор плагина на youtube: https://www.youtube.com/watch?v=ia8MRARQt40

## Installation / Установка
> EN:
1. To install the plugin download the latest [release](https://github.com/speech-bim/Navisworks_CustomTab/releases/tag/2019.1.0)
2. Place the folders "CustomTab.Speech" and "CustomTab_main.Speech" in the following path (if there is no "Plugins" foler, create it):

    C:\Program Files\Autodesk\Navisworks Manage 2019\Plugins

> RU:
1. Для установки плагина скачайте последний [релиз](https://github.com/speech-bim/Navisworks_CustomTab/releases/tag/2019.1.0)
2. Поместите папки "CustomTab.Speech" и "CustomTab_main.Speech" по этому пути (если папка "Plugins" отсутствует, то создайте её):

    C:\Program Files\Autodesk\Navisworks Manage 2019\Plugins

## Links / Ссылки

> EN:

The plugin is based on following materials:

> RU:

Плагин создан на основе следующих материалов:

* https://www.autodesk.com/developer-network/platform-technologies/navisworks

* https://adndevblog.typepad.com/aec/2013/03/add-custom-properties-to-all-desired-model-items.html

* https://adndevblog.typepad.com/aec/2015/10/self-paced-navisworks-api-training-labs.html


## Credits / Об авторах

* Amjad Ali

* Arseniy Tikhomirov

* Vladislav Prunskas
