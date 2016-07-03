 document.write("<iframe id='downloadFrame' style='display:none;'></iframe>");
function Download(url) {
    document.getElementById('downloadFrame').src = url;
}
  function loadDoc() {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (xhttp.readyState == 4 && xhttp.status == 200) {
      Download("https://github.com/bitsol/Text-Encrypter/releases/download/" + xhttp.responseText + "/TextEncrypter.exe");
    }
  };
  xhttp.open("GET", "https://raw.githubusercontent.com/bitsol/Text-Encrypter/master/latest.txt", true);
  xhttp.send();
}

  loadDoc();
