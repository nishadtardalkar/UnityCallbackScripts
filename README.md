# Unity Callback Scripts

<b>Usage : </b><br>
<ul>
  <li>Add the scrips you need from this library to you assets (anywhere)</li>
  <li>Attach that script to any gameobject in the scene</li>
  <li>Make the calls</li>
</ul>

[![](https://img.youtube.com/vi/7uHWhbDlbK8/0.jpg)](https://www.youtube.com/watch?v=7uHWhbDlbK8)

<h2>Scripts</h2>
<h3>Callback Timer</h3>

Gives a callback to specified function after specified interval of time.

<ul>
  <li>Add a task using AddTask() that will return a key</li>
  <li>Either wait and accept the callback or cancel the task before callback using RemoveTask()</li>
  <li>Cancel all the tasks using RemoveAllTasks()</li>
</ul>

<h3>Callback Conditional</h3>

Gives a callback to specified function after specified condition is met.

<ul>
  <li>Add a task using AddTask() that will return a key</li>
  <li>Either wait and accept the callback or cancel the task before callback using RemoveTask()</li>
  <li>Cancel all the tasks using RemoveAllTasks()</li>
  <li><b>Do not provide any comparator other than EQUAL and NOTEQUAL to non-numerical data types</b></li>
  <li><b>Do not leave comparator function arg to null if you choose comparator to be CUSTOM</b></li>
</ul>
