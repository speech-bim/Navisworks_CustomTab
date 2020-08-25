# Navisworks_CustomTab

## Description / Описание
> EN:

Navisworks plugin for automatic creation custom propeties tab based on txt file. This plugin finds an existing element's property, takes the value and creates a new property with that value. The plugin processes all elements in selected NWC or NWD file.

#### Instruction:

1. Run the "Custom Tab" plugin.
2. Select NWC or NWD file. 
3. Select TXT file with a list of new properties. TXT file structure must be:
* The first line is the name of the new tab;
* Each next line is a new property. At first name of the tab with original property is specified, then the name of original property, then the name of the new property. All names in line must be separated by tabs;
* If property from TXT file is missing, new property will not be created;
* If an element has a several original properties, that are suitable for creating a new parameter, then the first parameter from the list will be selected, the rest will be skipped;
* There may be blank lines between the lines with parameters.
4. Press "Старт".
5. Select the folder to saving the new file and enter the name of the new file.
6. After the "Файл готов" text appears you can open the new NWD file.

Plugin overview on youtube: https://www.youtube.com/channel/UC6fDdMZy6_jUKhOIp2FK2qQ

> RU:

Плагин Navisworks для автоматического создания вкладки пользовательских параметров на основе txt файла. Плагин находит существующий параметр элемента, берет его значение и создает новый параметр с этим значением. Плагин обрабывает все элементы в выбранном NWC или NWD файле.

#### Инструкция по работе:

1. Запустите плагин "Custom Tab" с вкладки "Надстройки инструментов 1".
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

Обзор плагина на youtube: https://www.youtube.com/channel/UC6fDdMZy6_jUKhOIp2FK2qQ

## Installation / Установка
> EN:
1. To install the plugin download the latest release 
2. Place folders "" and "" to this path (if there is no "Plugins" foler, create it):

    C:\Program Files\Autodesk\Navisworks Manage 2019\Plugins

> RU:
1. Для установки плагина скачайте последний релиз
2. Поместите папки "" и "" по этому пути (если папка "Plugins" отсутствует, то создайте её):

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
