// QEditor html validator. Returns true if html contains image or if innerText size >= minLength

export default function (el, minLength) {

  if (!el)
    return true;

  let img = el.querySelector("img");

  if (img != null)
    return true;

  let textLength = el.innerText.replace(/\s/g, "").length;

  return textLength >= minLength;
};
