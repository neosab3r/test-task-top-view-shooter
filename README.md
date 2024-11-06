Основные требования:
 - [✔] За одного игрока играет пользователь, за другого компьютер.
 - [✔] Игровое поле - прямоугольной формы. На нем расположены препятствия. Поле не огорожено
 - [✔] Игрок может перемещяться вперед/назад, разворачиваться вокруг своей оси влево/вправо. Также осуществлять выстрел. Пользователь это осуществляет с помощью клавиатуры, его противник - под управлением программы.
 - [✔] При попадании в одного из игроков наращивается количество очков у попавшего, игра возобновляется на исходных позициях. Очки отображаются на экране.
 - [✔] При попадании в препятствие снаряд отскакивает и летит дальше. Если снаряд покидает пределы поля, он исчезает.
 - [✔] Для игрока, управляемого компьютером, необходимо реализовать простое принятие решений по перемещениям, развороту, а также прицеливание и принятие решения о выстреле.

Общие требования:
 1. Визуализацию сделать достаточно схематичной.
    - Кубы и сферы.
 3. Важно сделать упор на архитектуру - разнести визуализацию, логику и т.д.
    - Для кор логики используется MVP подход (пр. BotSystem -> BotModel -> BotView). Отдельно для ботов реализован FSM для принятия решений.
    - UI логика реализована в менеджерах (пр. TipManagerView, MenuManagerView).
    - За запуск игры, конец раунда отвечает GameManager. 
 4. Меню, возможность повторной игры, хранение конфигурации в файлах - реализовывать не требуется (может быть реализовано по желанию).
    - Сделано только простое Меню.

Дополнительные требования:
  1. [❌] Реализовать алгоритм прицеливания у игрока, управляемого компьютером, с учетом рикошетов от препятствий - чтобы компьютер предпринимал попытку поразить противника через последовательность рикошетов от препятствий.

  2. [✔] Для игрока, управляемого компьютером, последовательных действий, направленных на избегание опасности собственного поражения - например,  перемещение, разворот и выстрел с отскоком от препятствий.
     - Сделано Уклонение для ботов.


**Уклонение**:

![Movie_005](https://github.com/user-attachments/assets/e0417272-4cec-41cf-85a5-01c35f407953)


**Меню - Выбор сложности:**
![Movie_006](https://github.com/user-attachments/assets/6d4963eb-bec3-4444-967e-3eec10fbdf60)


**Патрулирование:**
![Movie_008](https://github.com/user-attachments/assets/b32bfca8-6e7e-4358-ade3-95e1b6f27ab6)


**Общий вид раунда:**
![Movie_009](https://github.com/user-attachments/assets/379ec01d-9f93-47e0-9997-9d14f944b954)
