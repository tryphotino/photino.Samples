import * as THREE from 'three';
import { FontLoader } from 'three/examples/jsm/loaders/FontLoader.js';
import { TextGeometry } from 'three/examples/jsm/geometries/TextGeometry.js';
//import { MeshFaceMaterial } from 'three/examples/jsm/geometries/MeshFaceMaterial.js';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { rawFont } from './myfont.js'

export default function ThreeEntryPoint(sceneRef) {
  const scene = new THREE.Scene();
  const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1500);
  camera.position.z = 5;
  const renderer = new THREE.WebGLRenderer();
  renderer.setSize(window.innerWidth, window.innerHeight);
  sceneRef.appendChild(renderer.domElement);
  let controls = new OrbitControls(camera, sceneRef);
  controls.target.set(0, 0, 0);
  controls.rotateSpeed = 0.5;
  controls.update();
  const geometry = new THREE.TorusKnotGeometry(15, 3.3, 800, 9, 4, 10);
  const material = new THREE.MeshStandardMaterial({
    color: 0xff1111,
    emissive: 0x111122,
    specular: 0xfff11f,
    metalness: .9,
    roughness: 0.5,
  });

  const mesh = new THREE.Mesh(geometry, material);

  mesh.scale.x = 0.1;
  mesh.scale.y = 0.1;
  mesh.scale.z = 0.1;

  scene.add(mesh);

  // FONTS

  let loader = new FontLoader();
  let loadedFont = loader.parse( rawFont );

    //text3d
    var materialFront = new THREE.MeshBasicMaterial({ color: 0xff1111 });
    var materialSide = new THREE.MeshBasicMaterial({ color: 0x115078 });
    var materialArray = [materialFront, materialSide];
    var textGeom = new TextGeometry("PHOTINO",
      {
        size: 8, height: 4, curveSegments: 1,
        font: loadedFont, weight: "bold", style: "normal",
        bevelThickness: 0.1, bevelSize: 0.5, bevelEnabled: true,
        material: 0, extrudeMaterial: 1
      });

    var textMesh = new THREE.Mesh(textGeom, materialArray);

    textGeom.computeBoundingBox();

    textMesh.position.x = mesh.position.x+2;
    textMesh.position.y = mesh.position.y+2;
    textMesh.position.z = mesh.position.z;

    textMesh.scale.x = 0.1;
    textMesh.scale.y = 0.1;
    textMesh.scale.z = 0.1;

    textMesh.rotation.x = -Math.PI / 50;
    scene.add(textMesh);
  

  const frontSpot = new THREE.SpotLight(0xeeeece);
  const frontSpot2 = new THREE.SpotLight(0xddddce);

  frontSpot.position.set(1000, 1000, 1000);
  frontSpot2.position.set(-500, -500, -500);
  scene.add(frontSpot);
  scene.add(frontSpot2);
  const animate = function () {
    requestAnimationFrame(animate);

    mesh.rotation.x += 0.005;
    mesh.rotation.y += 0.005;
    mesh.rotation.z += 0.005;

    renderer.render(scene, camera);
  };
  animate();
}