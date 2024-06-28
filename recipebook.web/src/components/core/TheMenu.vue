<template>
    <div>
      <div class="sm:hidden">
        <label for="Tab" class="sr-only">Tab</label>
    
        <select id="Tab" class="w-full rounded-md border-gray-200">
          <option>Settings</option>
          <option>Messages</option>
          <option>Archive</option>
          <option select>Notifications</option>
        </select>
      </div>
    
      <div class="hidden sm:block">
        <div class="border-b border-gray-200">
          <nav class="-mb-px flex gap-6" aria-label="Tabs">
            <a
              href="#"
              class="shrink-0 border-b-2 border-sky-500 px-1 pb-4 text-sm font-medium text-sky-600"
            >
              Home
            </a>
    
            <a
              href="#"
              class="shrink-0 border-b-2 border-transparent px-1 pb-4 text-sm font-medium text-gray-500 hover:border-gray-300 hover:text-gray-700"
            >
              Recipes
            </a>
    
            <a
              href="#"
              class="shrink-0 border-b-2 border-transparent px-1 pb-4 text-sm font-medium text-gray-500 hover:border-gray-300 hover:text-gray-700"
            >
              Profile
            </a>
          </nav>
        </div>
      </div>
    </div>
  </template>