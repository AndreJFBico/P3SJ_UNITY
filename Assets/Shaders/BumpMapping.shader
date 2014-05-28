Shader "Custom/BumpMapping" {
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {} 
      _BumpMap ("Normal Map", 2D) = "bump" {}
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
	  _Shininess ("Shininess", Range(1.0, 20.0)) = 5.0
   }
   SubShader {
      Pass {      
         Tags { "LightMode" = "ForwardBase" } 
            //ambient light + first light source
 
         GLSLPROGRAM

		 uniform sampler2D _MainTex;
         uniform sampler2D _BumpMap;	
         uniform vec4 _BumpMap_ST;
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;

         uniform vec3 _WorldSpaceCameraPos; 

         uniform mat4 _Object2World;
         uniform mat4 _World2Object;
         uniform vec4 _WorldSpaceLightPos0; 
		 uniform vec4 unity_Scale;

         uniform vec4 _LightColor0;

         varying vec4 position; 

         varying vec4 textureCoordinates; 
         varying mat3 localSurface2World;
		 varying vec3 lightVec;
		 varying vec3 halfVec;
		 varying float attenuation;
 
         #ifdef VERTEX
 
         attribute vec4 Tangent;
 
         void main()
         {                                
            mat4 modelMatrix = _Object2World;
			mat4 modelMatrixInverse = _World2Object * unity_Scale.w;

            localSurface2World[0] = normalize(vec3(
               modelMatrix * vec4(vec3(Tangent), 0.0))); //tangT

            localSurface2World[2] = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse)); //tangN

            localSurface2World[1] = normalize(
               cross(localSurface2World[2], localSurface2World[0]) 
               * Tangent.w); //tangB

            position = modelMatrix * gl_Vertex; //vertexposition

			vec3 L;
		
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               L = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);

               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;

               L = normalize(vertexToLightSource);
            }

			// transform light and half angle vectors by tangent basis
			vec3 v;
			v.x = dot(L,localSurface2World[0]);
			v.y = dot(L,localSurface2World[1]);
			v.z = dot(L,localSurface2World[2]);
			lightVec = normalize(v);

			/*------------------------*/

			vec3 E = normalize(-position);//eyespace a posição da camera é 0,0,0
			vec3 H = normalize(lightVec + E);

			v.x = dot(H, localSurface2World[0]);
			v.y = dot(H, localSurface2World[1]);
			v.z = dot(H, localSurface2World[2]);
			
			halfVec = normalize(v);

            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec4 encodedNormal = texture2D(_BumpMap, 
               _BumpMap_ST.xy * textureCoordinates.xy 
               + _BumpMap_ST.zw);

            vec3 localCoords = 
               vec3(2.0 * encodedNormal.ag - vec2(1.0), 0.0); // vec3 N

            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));

            vec3 N = normalize(localSurface2World * localCoords);
 
			vec3 ambientLight = vec3(gl_LightModel.ambient) * vec3(_Color);
 
			float NdotL = clamp(dot(N,lightVec),0.0,1.0);
			vec3 diffuseLight = vec3(_LightColor0) * vec3(_Color) * NdotL;

			
			vec3 specularLight = vec3(0.0);
		
			if(NdotL > 0.0){
				vec3 R = reflect(-lightVec,N);
				float NdotH = clamp(dot(R,halfVec),0.0,1.0);
				float Blinn = pow(NdotH, _Shininess);
				specularLight = vec3(_LightColor0) * vec3(_SpecColor) * Blinn;
			}

			gl_FragColor = texture2D(_MainTex, vec2(textureCoordinates)) * vec4(ambientLight
               + (diffuseLight + specularLight)*attenuation, 1.0);
	
         }
 
         #endif
 
         ENDGLSL
      }

	  Pass {      
         Tags { "LightMode" = "ForwardAdd" } 
            //pass for additional light sources
		 Blend One One //additive blending
 
         GLSLPROGRAM

		 uniform sampler2D _MainTex;
         uniform sampler2D _BumpMap;	
         uniform vec4 _BumpMap_ST;
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;

         uniform vec3 _WorldSpaceCameraPos; 

         uniform mat4 _Object2World;
         uniform mat4 _World2Object;
         uniform vec4 _WorldSpaceLightPos0; 
		 uniform vec4 unity_Scale;

         uniform vec4 _LightColor0;

         varying vec4 position; 

         varying vec4 textureCoordinates; 
         varying mat3 localSurface2World;
		 varying vec3 lightVec;
		 varying vec3 halfVec;
		 varying float attenuation;
 
         #ifdef VERTEX
 
         attribute vec4 Tangent;
 
         void main()
         {                                
            mat4 modelMatrix = _Object2World;
			mat4 modelMatrixInverse = _World2Object * unity_Scale.w;

            localSurface2World[0] = normalize(vec3(
               modelMatrix * vec4(vec3(Tangent), 0.0))); //tangT
            localSurface2World[2] = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse)); //tangN
            localSurface2World[1] = normalize(
               cross(localSurface2World[2], localSurface2World[0]) 
               * Tangent.w); //tangB

            position = modelMatrix * gl_Vertex; //vertexposition

			vec3 L;
		
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               L = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);

               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;

               L = normalize(vertexToLightSource);
            }

			// transform light and half angle vectors by tangent basis
			vec3 v;
			v.x = dot(L,localSurface2World[0]);
			v.y = dot(L,localSurface2World[1]);
			v.z = dot(L,localSurface2World[2]);
			lightVec = normalize(v);

			/*------------------------*/

			vec3 E = normalize(-position);//eyespace a posição da camera é 0,0,0
			vec3 H = normalize(lightVec + E);

			v.x = dot(H, localSurface2World[0]);
			v.y = dot(H, localSurface2World[1]);
			v.z = dot(H, localSurface2World[2]);
			
			halfVec = normalize(v);

            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec4 encodedNormal = texture2D(_BumpMap, 
               _BumpMap_ST.xy * textureCoordinates.xy 
               + _BumpMap_ST.zw);

            vec3 localCoords = 
               vec3(2.0 * encodedNormal.ag - vec2(1.0), 0.0); // vec3 N

            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));

            vec3 N = normalize(localSurface2World * localCoords);
 

			float NdotL = clamp(dot(N,lightVec),0.0,1.0);
			vec3 diffuseLight = vec3(_LightColor0) * vec3(_Color) * NdotL;

			
			vec3 specularLight = vec3(0.0);
		
			if(NdotL > 0.0){
				vec3 R = reflect(-lightVec,N);
				float NdotH = clamp(dot(R,halfVec),0.0,1.0);
				float Blinn = pow(NdotH, _Shininess);
				specularLight = vec3(_LightColor0) * vec3(_SpecColor) * Blinn;
			}

			gl_FragColor = texture2D(_MainTex, vec2(textureCoordinates)) * vec4((diffuseLight + specularLight)*attenuation, 1.0);
	
         }
 
         #endif
 
         ENDGLSL
      }
   }

}

