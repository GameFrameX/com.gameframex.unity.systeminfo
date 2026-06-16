mergeInto(LibraryManager.library, {
  gameframex_webgl_device_unique_id: function () {
    var storageKey = "GameFrameX.DeviceUniqueIdentifier";
    var id = "";

    function generateId() {
      var bytes = new Uint8Array(16);

      if (typeof crypto !== "undefined" && crypto.getRandomValues) {
        crypto.getRandomValues(bytes);
      } else {
        for (var i = 0; i < bytes.length; i++) {
          bytes[i] = Math.floor(Math.random() * 256);
        }
      }

      var value = "";
      for (var j = 0; j < bytes.length; j++) {
        value += ("0" + bytes[j].toString(16)).slice(-2);
      }

      return value.toUpperCase();
    }

    try {
      id = window.localStorage.getItem(storageKey) || "";
    } catch (e) {
      id = "";
    }

    if (!id || id === "null" || id.length < 4) {
      id = generateId();

      try {
        window.localStorage.setItem(storageKey, id);
      } catch (e) {
      }
    }

    var bufferSize = lengthBytesUTF8(id) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(id, buffer, bufferSize);
    return buffer;
  }
});
